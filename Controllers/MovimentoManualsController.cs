using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SinqiaParibas.Models;
using SinqiaParibas.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SinqiaParibas.Controllers
{
    public class MovimentoManualsController : Controller
    {
        private readonly AppDbContext _context;

        public MovimentoManualsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: MovimentoManuals

        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.MovimentosManuais.Include(m => m.ProdutoCosif);

            var viewModel = new MovimentoManualViewModel
            {
                Movimentos = await appDbContext.ToListAsync(),//_context.MovimentoManuals.ToList(),
                MovimentoAtual = new MovimentoManual { COD_USUARIO = "TESTE" }
            };
            ViewData["Produtos"] = new SelectList(_context.Produtos, "COD_PRODUTO", "DES_PRODUTO");
            ViewData["ProdutoCosifs"] = new SelectList(_context.ProdutoCosifs, "COD_COSIF", "COD_CLASSIFICACAO");
            
            return View(viewModel);

          
        }

        // GET: MovimentoManuals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movimentoManual = await _context.MovimentosManuais
                .Include(m => m.ProdutoCosif)
                .FirstOrDefaultAsync(m => m.DAT_MES == id);
            if (movimentoManual == null)
            {
                return NotFound();
            }

            return View(movimentoManual);
        }

        public IActionResult Create()
        {
            ViewData["Produtos"] = new SelectList(_context.Produtos, "COD_PRODUTO", "DES_PRODUTO");
            ViewData["ProdutoCosifs"] = new SelectList(_context.ProdutoCosifs, "COD_COSIF", "COD_CLASSIFICACAO");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( MovimentoManualViewModel movimentoManual)
        {
          
                try
                {
                    movimentoManual.MovimentoAtual.DAT_MOVIMENTO = DateTime.Now;
                    movimentoManual.MovimentoAtual.COD_USUARIO = "TESTE";

                    var ultimoLancamento = _context.MovimentosManuais
                        .Where(m => m.DAT_MES == movimentoManual.MovimentoAtual.DAT_MES && m.DAT_ANO == movimentoManual.MovimentoAtual.DAT_ANO)
                        .OrderByDescending(m => m.NUM_LANCAMENTO)
                        .FirstOrDefault();

                    movimentoManual.MovimentoAtual.NUM_LANCAMENTO = (ultimoLancamento?.NUM_LANCAMENTO ?? 0) + 1;

                    var movimentoManualEntity = new MovimentoManual
                    {
                        DAT_MES = movimentoManual.MovimentoAtual.DAT_MES,
                        DAT_ANO = movimentoManual.MovimentoAtual.DAT_ANO,
                        NUM_LANCAMENTO = movimentoManual.MovimentoAtual.NUM_LANCAMENTO,
                        COD_PRODUTO = movimentoManual.MovimentoAtual.COD_PRODUTO,
                        COD_COSIF = movimentoManual.MovimentoAtual.COD_COSIF,
                        DES_DESCRICAO = movimentoManual.MovimentoAtual.DES_DESCRICAO,
                        DAT_MOVIMENTO = movimentoManual.MovimentoAtual.DAT_MOVIMENTO,
                        COD_USUARIO = movimentoManual.MovimentoAtual.COD_USUARIO,
                        VAL_VALOR = movimentoManual.MovimentoAtual.VAL_VALOR
                    };

                    _context.MovimentosManuais.Add(movimentoManualEntity);
                    await _context.SaveChangesAsync();

                // Recarrega os dados para o grid
                var movimentos = await _context.MovimentosManuais
                    .Include(m => m.ProdutoCosif.Produto) // Ajuste conforme os relacionamentos
                    .ToListAsync();

                return PartialView("_MovimentosGrid", movimentos);


                }
                catch (Exception ex)
                {
                    // Log do erro para depuração
                    return BadRequest($"Erro ao salvar no banco: {ex.Message}");
                }
        
        }
        public async Task<IActionResult> GetGrid()
        {
            var movimentos = await _context.MovimentosManuais.ToListAsync(); // Busque os dados necessários
            return PartialView("_MovimentosGrid", movimentos); // Retorne a partial com os dados
        }



        // GET: MovimentoManuals/Edit/5
        public async Task<IActionResult> Edit(int? mes, int? ano, int? lancamento)
        {
            if (mes == null || ano == null || lancamento == null)
            {
                return NotFound();
            }

            var movimentoManual =  _context.MovimentosManuais.FirstOrDefault(m => m.DAT_MES == mes && m.DAT_ANO == ano && m.NUM_LANCAMENTO == lancamento);
            if (movimentoManual == null)
            {
                return NotFound();
            }
            ViewData["COD_PRODUTO"] = new SelectList(_context.ProdutoCosifs, "COD_PRODUTO", "COD_PRODUTO", movimentoManual.COD_PRODUTO);
            return View(movimentoManual);
        }

        // POST: MovimentoManuals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DAT_MES,DAT_ANO,NUM_LANCAMENTO,COD_PRODUTO,COD_COSIF,DES_DESCRICAO,DAT_MOVIMENTO,COD_USUARIO,VAL_VALOR")] MovimentoManual movimentoManual)
        {
            if (id != movimentoManual.DAT_MES)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movimentoManual);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovimentoManualExists(movimentoManual.DAT_MES))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["COD_PRODUTO"] = new SelectList(_context.ProdutoCosifs, "COD_PRODUTO", "COD_PRODUTO", movimentoManual.COD_PRODUTO);
            return View(movimentoManual);
        }

        // GET: MovimentoManuals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movimentoManual = await _context.MovimentosManuais
                .Include(m => m.ProdutoCosif)
                .FirstOrDefaultAsync(m => m.DAT_MES == id);
            if (movimentoManual == null)
            {
                return NotFound();
            }

            return View(movimentoManual);
        }

        // POST: MovimentoManuals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movimentoManual = await _context.MovimentosManuais.FindAsync(id);
            if (movimentoManual != null)
            {
                _context.MovimentosManuais.Remove(movimentoManual);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovimentoManualExists(int id)
        {
            return _context.MovimentosManuais.Any(e => e.DAT_MES == id);
        }
    }
}
