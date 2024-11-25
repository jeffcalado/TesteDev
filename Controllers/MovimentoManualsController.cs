using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SinqiaMVC.Models;
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
            return View(await appDbContext.ToListAsync());
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

        // GET: MovimentoManuals/Create
        // public IActionResult Create()
        // {


        // ViewData["COD_PRODUTO"] = new SelectList(_context.ProdutoCosifs, "COD_PRODUTO", "COD_PRODUTO");
        // return View();
        //}
        public IActionResult Create()
        {
            ViewData["Produtos"] = new SelectList(_context.Produtos, "COD_PRODUTO", "DES_PRODUTO");
            ViewData["ProdutoCosifs"] = new SelectList(_context.ProdutoCosifs, "COD_COSIF", "COD_CLASSIFICACAO");
            return View();
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DAT_MES,DAT_ANO,NUM_LANCAMENTO,COD_PRODUTO,COD_COSIF,DES_DESCRICAO,DAT_MOVIMENTO,COD_USUARIO,VAL_VALOR")] MovimentoManual movimentoManual)
        {
           // if (ModelState.IsValid)
           // {
                movimentoManual.DAT_MOVIMENTO = DateTime.Now;
                movimentoManual.COD_USUARIO = "TESTE";

                var ultimoLancamento = _context.MovimentosManuais
                    .Where(m => m.DAT_MES == movimentoManual.DAT_MES && m.DAT_ANO == movimentoManual.DAT_ANO)
                    .OrderByDescending(m => m.NUM_LANCAMENTO)
                    .FirstOrDefault();

                movimentoManual.NUM_LANCAMENTO = (ultimoLancamento?.NUM_LANCAMENTO ?? 0) + 1;
                _context.Add(movimentoManual);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            //}
            //ViewData["COD_PRODUTO"] = new SelectList(_context.ProdutoCosifs, "COD_PRODUTO", "COD_PRODUTO", movimentoManual.COD_PRODUTO);
            //return View(movimentoManual);
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
