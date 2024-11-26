

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SinqiaParibas.Controllers;
using SinqiaParibas.Models;
using SinqiaParibas.ViewModels;
using System.Collections;
using Xunit;

public class MovimentoManualsControllerTests
{
    private readonly AppDbContext _context;
    private readonly MovimentoManualsController _controller;

    public MovimentoManualsControllerTests()
    {
       
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("TestDatabase")
            .Options;

        _context = new AppDbContext(options); 
        _controller = new MovimentoManualsController(_context);
    }

    [Fact]
    public async Task Create_InvalidModel_ReturnsBadRequest()
    {
        // Arrange
        _controller.ModelState.AddModelError("COD_PRODUTO", "O campo COD_PRODUTO é obrigatório.");

        var mov = new MovimentoManual {
            DAT_MES = 1,
            DAT_ANO = 2024,
            COD_PRODUTO = null, // Propriedade obrigatória omitida
            COD_COSIF = null,   // Propriedade obrigatória omitida
            DES_DESCRICAO = null, // Propriedade obrigatória omitida
            VAL_VALOR = 100
        };

        var movimento = new MovimentoManualViewModel
        {
            MovimentoAtual = mov 
        };

        // Act
        var result = await _controller.Create(movimento);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Modelo inválido", badRequestResult.Value);
    }


}



