using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using OrderManagement.Models;

namespace OrderManagement.Controllers
{
    public class OrdersController : Controller
    {
        private OrderContext db = new OrderContext();

        // GET: Orders
        public ActionResult Index()
        {
            return View(db.Orders.ToList());
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            // 明細行の行数
            ViewBag.DetailsCount = 1;
            return View();
        }

        // POST: Orders/Create
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,OrderDate,TableNumber,Details")] Order order, string submit, int detailsCount)
        {
            // submitには押されたボタンのvalueが、
            // detailsCountにはhiddenに格納された明細行数が格納されます

            if (submit == "Add Details")
            {
                // 明細追加が押された
                ViewBag.DetailsCount = detailsCount + 1;
                order.Details.Add(new Detail());
                return View(order);
            }

            if (ModelState.IsValid)
            {
                order.OrderDate = DateTime.Now;

                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(order);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            order.Details.Add(new Detail() { OrderId = id.Value });
            return View(order);
        }

        // POST: Orders/Edit/5
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,OrderDate,TableNumber,Details")] Order order, string submit)
        {
            // submitには押されたボタンのvalueが格納されます

            if (submit == "Add Details")
            {
                // 明細追加が押された
                order.Details.Add(new Detail() { OrderId = order.Id });
                return View(order);
            }

            if (ModelState.IsValid)
            {
                var list = order.Details.Where(item => item.Id == 0).ToList();
                foreach (var item in list)
                {
                    db.Details.Add(item);
                }

                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);

            db.Details.RemoveRange(order.Details);

            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
