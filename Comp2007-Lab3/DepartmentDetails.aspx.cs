using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

// using statements required for EF DB access
using Comp2007_Lab3.Models;
using System.Web.ModelBinding;
namespace Comp2007_Lab3 {
    public partial class DepartmentDetails : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            var customCulture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            var nfi = (NumberFormatInfo)customCulture.NumberFormat.Clone();
            nfi.CurrencyDecimalDigits = 2;
            customCulture.NumberFormat = nfi;
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;
            if ((!IsPostBack) && (Request.QueryString.Count > 0)) {
                this.GetDepartment();

            }
        }

        protected void GetDepartment() {
            //populate form with existing student record
            int DepartmentID = Convert.ToInt32(Request.QueryString["DepartmentID"]);
            //connect to database with ef
            using (DefaultConn db = new DefaultConn()) {
                //populate student instance with student id with url param
                Department updatedDepartment = (from department in db.Departments
                                          where department.DepartmentID == DepartmentID
                                          select department).FirstOrDefault();

                if (updatedDepartment != null) {
                    NameTextBox.Text = updatedDepartment.Name.ToString();
                    BudgetTextBox.Text = updatedDepartment.Budget.ToString();
                }

            }
        }
        protected void CancelButton_Click(object sender, EventArgs e) {
            // Redirect back to Students page
            Response.Redirect("~/Departments.aspx");
        }

        protected void SaveButton_Click(object sender, EventArgs e) {
            // Use EF to connect to the server
            using (DefaultConn db = new DefaultConn()) {
                // use the Student model to create a new student object and
                // save a new record
                Department newDepartment = new Department();

                int DepartmentID = 0;

                if (Request.QueryString.Count > 0) // our URL has a DepartmentID in it
                {
                    // get the id from the URL
                    DepartmentID = Convert.ToInt32(Request.QueryString["DepartmentID"]);

                    // get the current student from EF DB
                    newDepartment = (from department in db.Departments
                                  where department.DepartmentID == DepartmentID
                                  select department).FirstOrDefault();
                }

                // add form data to the new student record
                newDepartment.Name = NameTextBox.Text;
                newDepartment.Budget = Convert.ToDecimal(BudgetTextBox.Text);

                // use LINQ to ADO.NET to add / insert new student into the database

                if (DepartmentID == 0) {
                    db.Departments.Add(newDepartment);
                }


                // save our changes - also updates and inserts
                db.SaveChanges();

                // Redirect back to the updated students page
                Response.Redirect("~/Departments.aspx");
            }
        }
    }
}