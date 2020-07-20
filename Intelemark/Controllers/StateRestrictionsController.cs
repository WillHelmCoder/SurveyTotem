using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Intelemark.Entities;
using Intelemark.Models;

namespace Intelemark.Controllers
{
    [Authorize]
    public class StateRestrictionsController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public async Task<ActionResult> Index()
        {
            try
            {
                var entities = await db.StateRestrictions.Where(x => !x.IsDeleted).ToListAsync();
                var models = new List<StateRestrictionModel>();
                foreach (var entity in entities)
                    models.Add(new StateRestrictionModel
                    {
                        Id = entity.Id,
                        Abbreviation = entity.Abbreviation,
                        Name = entity.Name,
                        IsRestricted = entity.IsRestricted,
                        LastUpdate = entity.LastUpdate,
                        CreationDate = entity.CreationDate,
                    });

                return View(models);
            }
            catch (Exception e)
            {
                AddAlert($"Oops! something went wrong. Error code: {e.HResult}", "Index", this.GetType().ToString(), AlertType.error, e);
                return View();
            }
        }

        public async Task<ActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                var entity = await db.StateRestrictions.FindAsync(id);
                if (entity == null)
                    return HttpNotFound();

                var model = new StateRestrictionModel
                {
                    Abbreviation = entity.Abbreviation,
                    Name = entity.Name,
                    IsRestricted = entity.IsRestricted,
                    LastUpdate = entity.LastUpdate
                };
                return View(model);
            }
            catch (Exception e)
            {
                AddAlert($"Oops! something went wrong. Error code: {e.HResult}", "Edit", this.GetType().ToString(), AlertType.error, e);
                return View(new FormModel());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(StateRestrictionModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entity = await db.StateRestrictions.FindAsync(model.Id);

                    if (entity == null)
                        return HttpNotFound();

                    entity.IsRestricted = model.IsRestricted;
                    entity.LastUpdate = DateTime.Now;

                    db.Entry(entity).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                return View(model);
            }
            catch (Exception e)
            {
                AddAlert($"Oops! something went wrong. Error code: {e.HResult}", "Edit(model)", this.GetType().ToString(), AlertType.error, e);
                return View(model);
            }

        }
    }
}
