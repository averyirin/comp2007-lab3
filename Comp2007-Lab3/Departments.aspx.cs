using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//required to connect to EF db
using Comp2007_Lab3.Models;
using System.Web.ModelBinding;
namespace Comp2007_Lab3 {
    public partial class Departments : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            // if loading the page for the first time, populate the student grid
            if (!IsPostBack) {
                //two session varibles to sort data

                Session["SortColumn"] = "DepartmentID";
                Session["SortDirection"] = "ASC";


                // Get the student data
                this.GetDepartments();

            }
        }

        /**
         * <summary>
         * This method gets the student data from the DB
         * </summary>
         * 
         * @method GetDepartments
         * @returns {void}
         */
        protected void GetDepartments() {
            // connect to EF
            using (DefaultConn db = new DefaultConn()) {
                //create query string
                string SortString = Session["SortColumn"].ToString() + " " + Session["SortDirection"].ToString();
                // query the Students Table using EF and LINQ
                var Students = (from allDepartments in db.Departments
                                select  allDepartments);

                // bind the result to the GridView
                DepartmentsGridView.DataSource = Students.AsQueryable().OrderBy(SortString).ToList();
                DepartmentsGridView.DataBind();
            }
        }

        /**
         * <summary>
         * This method is used to detelte student records from the database using ef
         * </summary>
         * 
         * @method DepartmentsGridView_RowDeleting
         * @param {object} sender
         * @param {GridViewDeleteEventArgs} e
         * @return {void}
         */
        protected void DepartmentsGridView_RowDeleting(object sender, GridViewDeleteEventArgs e) {
            // store which row was selected for deletion
            int selectedRow = e.RowIndex;

            // get the selected DepartmentID using the grid's Datakey collection
            int DepartmentID = Convert.ToInt32(DepartmentsGridView.DataKeys[selectedRow].Values["DepartmentID"]);

            // use EF to find the selected student from DB and Remove it
            using (DefaultConn db = new DefaultConn()) {
                Department deletedDepartment = (from departmentRecords in db.Departments
                                          where departmentRecords.DepartmentID == DepartmentID
                                          select departmentRecords).FirstOrDefault();

                // remove the student record from the database
                db.Departments.Remove(deletedDepartment);

                // save changes to the db
                db.SaveChanges();

                // refresh the grid
                this.GetDepartments();
            }
        }

        /**
     * <summary>
     * This method is used for paging
     * </summary>
     * 
     * @method DepartmentsGridView_PageIndexChanging
     * @param {object} sender
     * @param {GridViewDeleteEventArgs} e
     * @return {void}
     */
        protected void DepartmentsGridView_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            //set new page number
            DepartmentsGridView.PageIndex = e.NewPageIndex;
            this.GetDepartments();

        }

        protected void DepartmentsDropDownList_SelectedIndexChanged(object sender, EventArgs e) {
            //set new page size
            DepartmentsGridView.PageSize = Convert.ToInt32(DepartmentsDropDownList.SelectedValue);
            //refresh grid
            this.GetDepartments();
        }

        protected void DepartmentsGridView_Sorting(object sender, GridViewSortEventArgs e) {
            //get the column to sort by
            Session["SortColumn"] = e.SortExpression;
            this.GetDepartments();

            //toggle the sort direction
            Session["SortDirection"] = Session["SortDirection"].ToString() == "ASC" ? "DESC" : "ASC";
        }

        protected void DepartmentsGridView_RowDataBound(object sender, GridViewRowEventArgs e) {
            if (IsPostBack) {
                if (e.Row.RowType == DataControlRowType.Header) // if the row clicked is the header row
                {
                    LinkButton linkButton = new LinkButton();

                    for (int index = 0; index < DepartmentsGridView.Columns.Count; index++) // check each column for a click
                    {
                        if (DepartmentsGridView.Columns[index].SortExpression == Session["SortColumn"].ToString()) {
                            if (Session["SortDirection"].ToString() == "ASC") {
                                linkButton.Text = " <i class='fa fa-caret-up fa-lg'></i>";
                            }
                            else {
                                linkButton.Text = " <i class='fa fa-caret-down fa-lg'></i>";
                            }

                            e.Row.Cells[index].Controls.Add(linkButton); // add the new linkbutton to header cell
                        }
                    }
                }
            }
        }
    }
}