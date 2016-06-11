<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DepartmentDetails.aspx.cs" Inherits="Comp2007_Lab3.DepartmentDetails" %>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <div class="container">
        <div class="row">
            <div class="col-md-offset-3 col-md-6">
                <h1>Department Details</h1>
                <h5>All Fields are Required</h5>
                <br />
                <div class="form-group">
                    <label class="control-label" for="NameTextBox">Name</label>
                    <asp:TextBox runat="server" CssClass="form-control" ID="NameTextBox" placeholder="Name" required="true"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label class="control-label" for="BudgetTextBox">Budget</label>
                    <asp:TextBox runat="server" TextMode="Number" CssClass="form-control" ID="BudgetTextBox" placeholder="0" required="true"></asp:TextBox>
                    <asp:RangeValidator ID="RangeValidator" runat="server" ErrorMessage="Invalid Currency! Format: $"
                        ControlToValidate="BudgetTextBox" MinimumValue="0.00"  MaximumValue="1000000.00"
                        Type="Currency" Display="Dynamic" BackColor="Red" ForeColor="White" Font-Size="Large"></asp:RangeValidator>
                </div>
                <div class="text-right">
                    <asp:Button Text="Cancel" ID="CancelButton" CssClass="btn btn-warning btn-lg" runat="server" 
                        UseSubmitBehavior="false" CausesValidation="false" OnClick="CancelButton_Click" />
                    <asp:Button Text="Save" ID="SaveButton" CssClass="btn btn-primary btn-lg" runat="server" OnClick="SaveButton_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>