using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using evaluacion20262.Data;
using evaluacion20262.Models;

namespace evaluacion20262.Controllers;

public class SolicitudServicioController : Controller
{
    private readonly ApplicationDbContext _context;

    public SolicitudServicioController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var solicitudes = await _context.SolicitudesServicio
            .OrderByDescending(s => s.FechaRegistro)
            .ToListAsync();

        return View(solicitudes);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(SolicitudServicio solicitud)
    {
        if (ModelState.IsValid)
        {
            try
            {
                solicitud.FechaRegistro = DateTime.Now;
                _context.Add(solicitud);
                _context.SaveChanges();

                TempData["Mensaje"] = "Solicitud registrada correctamente.";
                return RedirectToAction(nameof(Create));
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Ocurrió un error al guardar la solicitud. Inténtelo nuevamente.");
            }
        }

        return View(solicitud);
    }
}