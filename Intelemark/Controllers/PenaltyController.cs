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
using Intelemark.Services;

namespace Intelemark.Controllers
{
    [Authorize]
    public class PenaltyController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public async Task<ActionResult> Index()
        {
            try
            {
                var entities = await db.Penalties.Where(x => !x.IsDeleted).ToListAsync();
                var models = new List<PenaltyModel>();
                foreach (var entity in entities)
                    models.Add(new PenaltyModel
                    {
                        Id = entity.Id,
                        PayRate = entity.PayRate,
                        From = entity.From,
                        To = entity.To,
                        PenaltyFee = entity.PenaltyFee,
                        LastUpdate = entity.LastUpdate,
                        CreationDate = entity.CreationDate
                    });

                return View(models);
            }
            catch (Exception e)
            {
                AddAlert($"Oops! something went wrong. Error code: {e.HResult}", "Index", this.GetType().ToString(), AlertType.error, e);
                return View();
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(PenaltyModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entity = new Penalty
                    {
                        PayRate = model.PayRate,
                        From = model.From,
                        To = model.To,
                        PenaltyFee = model.PenaltyFee,
                        LastUpdate = DateTime.Now,
                        CreationDate = DateTime.Now,
                        IsDeleted = false,
                    };

                    db.Penalties.Add(entity);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                AddAlert($"Oops! something went wrong. Error code: {e.HResult}", "Create(model)", this.GetType().ToString(), AlertType.error, e);
            }

            return View(model);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                var entity = await db.Penalties.FindAsync(id);
                if (entity == null)
                    return HttpNotFound();

                var model = new PenaltyModel
                {
                    PayRate = entity.PayRate,
                    From = entity.From,
                    To = entity.To,
                    PenaltyFee = entity.PenaltyFee,
                };

                return View(model);

            }
            catch (Exception e)
            {
                AddAlert($"Oops! something went wrong. Error code: {e.HResult}", "Edit", this.GetType().ToString(), AlertType.error, e);
                return View(new ProjectModel());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(PenaltyModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entity = await db.Penalties.FindAsync(model.Id);

                    if (entity == null)
                        return HttpNotFound();

                    entity.PayRate = model.PayRate;
                    entity.From = model.From;
                    entity.To = model.To;
                    entity.PenaltyFee = model.PenaltyFee;
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

        public async Task<ActionResult> Delete(int id)
        {
            var res = await Service<Penalty>.Delete(new Penalty { Id = id });
            return RedirectToAction("Index");
        }
    }
}
