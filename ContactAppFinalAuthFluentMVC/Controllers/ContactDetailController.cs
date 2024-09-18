using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ContactAppFinalAuthFluentMVC.Data;
using ContactAppFinalAuthFluentMVC.Models;

namespace ContactAppFinalAuthFluentMVC.Controllers
{
    public class ContactDetailController : Controller
    {
        // GET: ContactDetail
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetContactDetails(int contactId)
        {
            Session["contactId"] = contactId;
            using (var session = NHibernateHelper.CreateSession())
            {
                var details = session.Query<ContactDetail>().Where(cd => cd.Contact.Id == contactId).ToList();
                return View(details);
            }
        }


        public ActionResult GetDetails(int id)
        {
            Session["ContactId"] = id;
            return Content("<h1>Hello</h1>");
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]

        public ActionResult Create(ContactDetail contactDetail) {
            int id = (int)Session["contactId"];
            using (var session = NHibernateHelper.CreateSession())
            {
                using (var txn = session.BeginTransaction())
                {
                    contactDetail.Contact.Id = id;
                    session.Save(contactDetail);
                    txn.Commit();
                    return RedirectToAction("GetContactDetails", new { contactId = id });
                }
            }
        
        }

        public ActionResult Edit(int id)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                var detail = session.Get<ContactDetail>(id);
                return View(detail);
            }
        }

        [HttpPost]

        public ActionResult Edit(ContactDetail contactDetail) {
            int id = (int)Session["contactId"];
            using (var session = NHibernateHelper.CreateSession())
            {
                using (var txn = session.BeginTransaction())
                {
                   contactDetail.Contact.Id = id;  
                    session.Update(contactDetail);
                    txn.Commit();
                    return RedirectToAction("GetContactDetails", new { contactId = id });
                }
            }
        
        }

        public ActionResult Delete(int id)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                var detail = session.Get<ContactDetail>(id);
                return View(detail);
            }
        }

        [HttpPost,ActionName("Delete")]
        public ActionResult DeleteContactDetail(int id)
        {
            int contactId = (int)Session["contactId"];
            using ( var session = NHibernateHelper.CreateSession())
            {
                using (var txn = session.BeginTransaction())
                {
                    var details = session.Get<ContactDetail>(id);
                    session.Delete(details);
                    txn.Commit();
                    return RedirectToAction("GetContactDetails", new { contactId = contactId });
                }
            }
        }
    }
}