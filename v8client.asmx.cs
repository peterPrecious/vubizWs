using DocumentFormat.OpenXml.Drawing;
using hiveCat.appBuilder.Emailers;
using hiveCat.appBuilder.ExcelWriter;
using ImageMagick;

using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Services;

namespace vubizWS
{
  [WebService(Namespace = "http://vubiz.com/")]
  [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
  [System.ComponentModel.ToolboxItem(false)]
  [System.Web.Script.Services.ScriptService]

  public class v8client : System.Web.Services.WebService
  { // these client side web services are called via AJAX

    //adding a meaningless comment to test GIT


    [WebMethod]
    public void accountActivity(string custId, string certPrograms, string certPrograms_E)
    {
      SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString);
      con.Open();
      SqlCommand cmd = new SqlCommand
      {
        Connection = con,
        CommandText = "dbo.sp7accountActivity",
        CommandType = CommandType.StoredProcedure
      };
      cmd.Parameters.Add(new SqlParameter("@custId", custId));
      cmd.Parameters.Add(new SqlParameter("@certPrograms", certPrograms));
      cmd.Parameters.Add(new SqlParameter("@certPrograms_E", certPrograms_E));
      SqlDataReader drd = cmd.ExecuteReader();
      string result = "null";
      if (drd.HasRows)
      {
        result = convertToJSON(drd, false);
      }
      con.Close();
      Context.Response.Write(result);
    }

    [WebMethod]
    public void authenticate(int applicationId, string userName, string membPassword)
    {
      // appId=1 : username has the format ABCD1234\myusername
      SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString);
      con.Open();
      SqlCommand cmd = new SqlCommand();
      cmd.Connection = con;
      cmd.CommandText = "dbo.sp8authenticate";
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.Parameters.Add(new SqlParameter("@applicationId", applicationId));
      cmd.Parameters.Add(new SqlParameter("@userName", userName));
      cmd.Parameters.Add(new SqlParameter("@password", membPassword));
      SqlDataReader drd = cmd.ExecuteReader();
      string result = "null";
      if (drd.HasRows)
      {
        result = convertToJSON(drd, false);
      }
      con.Close();
      Context.Response.Write(result);
    }

    [WebMethod]
    public void browserCheck() // this returns the primary key whenever you check the browser - it ensures that AJAX works
    {
      SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString);
      con.Open();
      SqlCommand cmd = new SqlCommand();
      cmd.Connection = con;
      cmd.CommandText = "dbo.sp8browserCheck";
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.Parameters.Add("@browserNo", SqlDbType.Int, 1).Direction = ParameterDirection.Output;
      cmd.ExecuteNonQuery();
      string browserNo = cmd.Parameters["@browserNo"].Value.ToString();
      string result = "{\"browserNo\":\"" + browserNo + "\"}";
      Context.Response.Write(result);
    }

    [WebMethod]
    public void catalogue(string custId)
    {
      SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString);
      con.Open();
      SqlCommand cmd = new SqlCommand();
      cmd.Connection = con;
      cmd.CommandText = "dbo.sp8catalogue";
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.Parameters.Add(new SqlParameter("@custId", custId));
      SqlDataReader drd = cmd.ExecuteReader();
      string result = "null";
      if (drd.HasRows)
      {
        result = convertToJSON(drd, true);
      }
      con.Close();
      Context.Response.Write(result);
    }

    [WebMethod]
    public void credentials(string membEmail, string lang)
    {
      SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString);
      con.Open();
      SqlCommand cmd = new SqlCommand();
      cmd.Connection = con;
      cmd.CommandText = "dbo.sp8credentials";
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.Parameters.Add(new SqlParameter("@membEmail", membEmail));
      cmd.Parameters.Add(new SqlParameter("@lang", lang));
      SqlDataReader drd = cmd.ExecuteReader();
      string result = "null";
      if (drd.HasRows)
      {
        result = convertToJSON(drd, true);
      }
      con.Close();
      Context.Response.Write(result);
    }

    [WebMethod]
    public void v8credentials(string custId, string membEmail)
    {
      SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString);
      con.Open();
      SqlCommand cmd = new SqlCommand();
      cmd.Connection = con;
      cmd.CommandText = "dbo.sp8v8credentials";
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.Parameters.Add(new SqlParameter("@custId", custId));
      cmd.Parameters.Add(new SqlParameter("@membEmail", membEmail));
      SqlDataReader drd = cmd.ExecuteReader();
      string result = "null";
      if (drd.HasRows)
      {
        result = convertToJSON(drd, true);
      }
      con.Close();
      Context.Response.Write(result);
    }

    [WebMethod]
    public void customer_prev(string custId)
    {
      SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString);
      con.Open();
      SqlCommand cmd = new SqlCommand();
      cmd.Connection = con;
      cmd.CommandText = "dbo.sp8customer";
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.Parameters.Add(new SqlParameter("@custId", custId));
      SqlDataReader drd = cmd.ExecuteReader();
      string result = "null";
      if (drd.HasRows)
      {
        result = convertToJSON(drd, false);
      }
      con.Close();
      Context.Response.Write(result);
    }


    [WebMethod]
    public void customer(string custId)
    {
      SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString);
      con.Open();
      SqlCommand cmd = new SqlCommand();
      cmd.Connection = con;
      cmd.CommandText = "dbo.sp8customer";
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.Parameters.Add(new SqlParameter("@custId", custId));
      SqlDataReader drd = cmd.ExecuteReader();
      string result = "null";
      if (drd.HasRows)
      {
        result = convertToJSON(drd, false);
      }
      con.Close();
      Context.Response.Write(result);
    }



    [WebMethod]
    public void customerReport(string custId, string custAcctId, string custTitle, int custActive, bool excel, string membNo, string lang, string fileName, string reportName)
    {
      SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString);
      con.Open();
      SqlCommand cmd = new SqlCommand();
      cmd.Connection = con;
      cmd.CommandText = "dbo.sp7customerReport";
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.Parameters.Add(new SqlParameter("@custId", custId));
      cmd.Parameters.Add(new SqlParameter("@custAcctId", custAcctId));
      cmd.Parameters.Add(new SqlParameter("@custTitle", custTitle));
      cmd.Parameters.Add(new SqlParameter("@custActive", custActive));
      SqlDataReader drd = cmd.ExecuteReader();
      string result = "null";
      if (drd.HasRows) result = convertToJSON(drd, true);
      con.Close();
      if (excel)
      {
        ExcelReport report = null;
        report = new ExcelReport(membNo, lang, reportName);
        report.customerReport(result);
        Context.Response.Write("null");
      }
      else
      {
        Context.Response.Write(result);
      };
    }

    [WebMethod]
    public void email(string emailFrom, string emailTo, string emailSubject, string emailBody)
    {
      Emailer emailer = new Emailer();
      string result = emailer.sendMessage(emailFrom, emailTo, emailSubject, emailBody);
      result = "{ \"status\": \"" + result + "\"}";
      Context.Response.Write(result);
    }

    [WebMethod]
    public void deleteGuest(string membGuid)
    {
      SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString);
      con.Open();
      SqlCommand cmd = new SqlCommand();
      cmd.Connection = con;
      cmd.CommandText = "dbo.sp8deleteGuest";
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.Parameters.Add(new SqlParameter("@membGuid", membGuid));
      SqlDataReader drd = cmd.ExecuteReader();
      string result = "null";
      if (drd.HasRows)
      {
        result = convertToJSON(drd, false);
      }
      con.Close();
      Context.Response.Write(result);
    }

    [WebMethod]
    public void ecomRegister(string ecomStoreId, string ecomCustId, string ecomId, string ecomPwd, string ecomFirstName, string ecomLastName, string ecomEmail, string ecomOrganization, string ecomCountry, string ecomCountryId, string ecomProvince, string ecomProvinceId, string ecomAddress, string ecomCity, string ecomPostalZip, string ecomPhone, string ecomPhoneExt)
    {
      SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString);
      con.Open();
      SqlCommand cmd = new SqlCommand();
      cmd.Connection = con;
      cmd.CommandText = "dbo.sp7ecomRegister";
      cmd.CommandType = CommandType.StoredProcedure;

      cmd.Parameters.Add(new SqlParameter("ecomStoreId", ecomStoreId));
      cmd.Parameters.Add(new SqlParameter("ecomCustId", ecomCustId));
      cmd.Parameters.Add(new SqlParameter("ecomId", ecomId));
      cmd.Parameters.Add(new SqlParameter("ecomPwd", ecomPwd));
      cmd.Parameters.Add(new SqlParameter("ecomFirstName", ecomFirstName));
      cmd.Parameters.Add(new SqlParameter("ecomLastName", ecomLastName));
      cmd.Parameters.Add(new SqlParameter("ecomEmail", ecomEmail));

      cmd.Parameters.Add(new SqlParameter("ecomOrganization", ecomOrganization));
      cmd.Parameters.Add(new SqlParameter("ecomCountry", ecomCountry));
      cmd.Parameters.Add(new SqlParameter("ecomCountryId", ecomCountryId));
      cmd.Parameters.Add(new SqlParameter("ecomProvince", ecomProvince));
      cmd.Parameters.Add(new SqlParameter("ecomProvinceId", ecomProvinceId));

      cmd.Parameters.Add(new SqlParameter("ecomAddress", ecomAddress));
      cmd.Parameters.Add(new SqlParameter("ecomCity", ecomCity));
      cmd.Parameters.Add(new SqlParameter("ecomPostalZip", ecomPostalZip));
      cmd.Parameters.Add(new SqlParameter("ecomPhone", ecomPhone));
      cmd.Parameters.Add(new SqlParameter("ecomPhoneExt", ecomPhoneExt));

      SqlDataReader drd = cmd.ExecuteReader();
      string result = "null";
      if (drd.HasRows)
      {
        result = convertToJSON(drd, false);
      }
      con.Close();
      Context.Response.Write(result);
    }

    [WebMethod]
    public void ecomSignIn(string ecomId, string ecomPwd, int ecomStoreId)
    {
      SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString);
      con.Open();
      SqlCommand cmd = new SqlCommand();
      cmd.Connection = con;
      cmd.CommandText = "dbo.sp7ecomSignIn";
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.Parameters.Add(new SqlParameter("ecomId", ecomId));
      cmd.Parameters.Add(new SqlParameter("ecomPwd", ecomPwd));
      cmd.Parameters.Add(new SqlParameter("ecomStoreId", ecomStoreId));
      SqlDataReader drd = cmd.ExecuteReader();
      string result = "null";
      if (drd.HasRows)
      {
        result = convertToJSON(drd, false);
      }
      con.Close();
      Context.Response.Write(result);
    }

    [WebMethod]
    public void ecomAccountGet(string ecomGuid)
    {
      SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString);
      con.Open();
      SqlCommand cmd = new SqlCommand();
      cmd.Connection = con;
      cmd.CommandText = "dbo.sp7ecomAccountGet";
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.Parameters.Add(new SqlParameter("ecomGuid", ecomGuid));
      SqlDataReader drd = cmd.ExecuteReader();
      string result = "null";
      if (drd.HasRows)
      {
        result = convertToJSON(drd, false);
      }
      con.Close();
      Context.Response.Write(result);
    }

    [WebMethod]
    public void ecomAccountPut(string ecomGuid, string ecomFirstName, string ecomLastName, string ecomEmail, string ecomId, string ecomPwd, string ecomOrganization, string ecomCountry, string ecomCountryId, string ecomProvince, string ecomProvinceId, string ecomAddress, string ecomCity, string ecomPostalZip, string ecomPhone, string ecomPhoneExt, string ecomChildId)
    {
      SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString);
      con.Open();
      SqlCommand cmd = new SqlCommand();
      cmd.Connection = con;
      cmd.CommandText = "dbo.sp7ecomAccountPut";
      cmd.CommandType = CommandType.StoredProcedure;

      cmd.Parameters.Add(new SqlParameter("ecomGuid", ecomGuid));

      cmd.Parameters.Add(new SqlParameter("ecomFirstName", ecomFirstName));
      cmd.Parameters.Add(new SqlParameter("ecomLastName", ecomLastName));
      cmd.Parameters.Add(new SqlParameter("ecomEmail", ecomEmail));
      cmd.Parameters.Add(new SqlParameter("ecomId", ecomId));
      cmd.Parameters.Add(new SqlParameter("ecomPwd", ecomPwd));
      cmd.Parameters.Add(new SqlParameter("ecomOrganization", ecomOrganization));

      cmd.Parameters.Add(new SqlParameter("ecomCountry", ecomCountry));
      cmd.Parameters.Add(new SqlParameter("ecomCountryId", ecomCountryId));
      cmd.Parameters.Add(new SqlParameter("ecomProvince", ecomProvince));
      cmd.Parameters.Add(new SqlParameter("ecomProvinceId", ecomProvinceId));

      cmd.Parameters.Add(new SqlParameter("ecomAddress", ecomAddress));
      cmd.Parameters.Add(new SqlParameter("ecomCity", ecomCity));
      cmd.Parameters.Add(new SqlParameter("ecomPostalZip", ecomPostalZip));
      cmd.Parameters.Add(new SqlParameter("ecomPhone", ecomPhone));
      cmd.Parameters.Add(new SqlParameter("ecomPhoneExt", ecomPhoneExt));

      cmd.Parameters.Add(new SqlParameter("ecomChildId", ecomChildId));

      SqlDataReader drd = cmd.ExecuteReader();
      string result = "null";
      if (drd.HasRows)
      {
        result = convertToJSON(drd, false);
      }
      con.Close();
      Context.Response.Write(result);
    }

    [WebMethod]
    public void getGuest(int membNo)
    {
      SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString);
      con.Open();
      SqlCommand cmd = new SqlCommand();
      cmd.Connection = con;
      cmd.CommandText = "dbo.sp8getGuest";
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.Parameters.Add(new SqlParameter("@membNo", membNo));
      SqlDataReader drd = cmd.ExecuteReader();
      string result = "null";
      if (drd.HasRows)
      {
        result = convertToJSON(drd, false);
      }
      con.Close();
      Context.Response.Write(result);
    }

    [WebMethod]
    public void guest(string custId, string membEmail, string membPassword, string membFirstName, string membLastName, bool membActive, int membParent, string membOrganization)
    {
      SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString);
      con.Open();
      SqlCommand cmd = new SqlCommand();
      cmd.Connection = con;
      cmd.CommandText = "dbo.sp8guest";
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.Parameters.Add(new SqlParameter("@custId", custId));
      cmd.Parameters.Add(new SqlParameter("@membEmail", membEmail));
      cmd.Parameters.Add(new SqlParameter("@membPassword", membPassword));
      cmd.Parameters.Add(new SqlParameter("@membFirstName", membFirstName));
      cmd.Parameters.Add(new SqlParameter("@membLastName", membLastName));
      cmd.Parameters.Add(new SqlParameter("@membActive", membActive));
      cmd.Parameters.Add(new SqlParameter("@membParent", membParent));
      cmd.Parameters.Add(new SqlParameter("@membOrganization", membOrganization));
      SqlDataReader drd = cmd.ExecuteReader();
      string result = "null";
      if (drd.HasRows)
      {
        result = convertToJSON(drd, false);
      }
      con.Close();
      Context.Response.Write(result);
    }

    [WebMethod]
    public void guests(int membNo)
    {
      SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString);
      con.Open();
      SqlCommand cmd = new SqlCommand();
      cmd.Connection = con;
      cmd.CommandText = "dbo.sp8guests";
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.Parameters.Add(new SqlParameter("@membNo", membNo));
      SqlDataReader drd = cmd.ExecuteReader();
      string result = "null";
      if (drd.HasRows)
      {
        result = convertToJSON(drd, true);
      }
      con.Close();
      Context.Response.Write(result);
    }

    [WebMethod]
    public void guestSetup(string membGuid, string membCatalogue)
    {
      SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString);
      con.Open();
      SqlCommand cmd = new SqlCommand();
      cmd.Connection = con;
      cmd.CommandText = "dbo.sp8guestSetup";
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.Parameters.Add(new SqlParameter("@membGuid", membGuid));
      cmd.Parameters.Add(new SqlParameter("@membCatalogue", membCatalogue));
      SqlDataReader drd = cmd.ExecuteReader();
      string result = "null";
      if (drd.HasRows)
      {
        result = convertToJSON(drd, false);
      }
      con.Close();
      Context.Response.Write(result);
    }

    [WebMethod]
    public void historyPrograms(int membNo, int completed)
    {
      SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString);
      con.Open();
      SqlCommand cmd = new SqlCommand();
      cmd.Connection = con;
      cmd.CommandText = "dbo.sp8historyPrograms";
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.Parameters.Add(new SqlParameter("@membNo", membNo));
      cmd.Parameters.Add(new SqlParameter("@completed", completed));
      SqlDataReader drd = cmd.ExecuteReader();
      string result = "null";
      if (drd.HasRows)
      {
        result = convertToJSON(drd, true);
      }
      con.Close();
      Context.Response.Write(result);
    }

    [WebMethod]
    public void isGovOrganizations(string strDate, string endDate)
    {
      SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString);
      con.Open();
      SqlCommand cmd = new SqlCommand();
      cmd.Connection = con;
      cmd.CommandText = "dbo.sp8isGovOrganizations";
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.CommandTimeout = 0;
      cmd.Parameters.Add(new SqlParameter("@strDate", strDate));
      cmd.Parameters.Add(new SqlParameter("@endDate", endDate));
      SqlDataReader drd = cmd.ExecuteReader();
      string result = "null";
      if (drd.HasRows)
      {
        result = convertToJSON(drd, true);
      }
      con.Close();
      Context.Response.Write(result);
    }

    [WebMethod]
    public void historyProgram(int membNo, int progNo)
    {
      SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString);
      con.Open();
      SqlCommand cmd = new SqlCommand();
      cmd.Connection = con;
      cmd.CommandText = "dbo.sp8historyProgram";
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.Parameters.Add(new SqlParameter("@membNo", membNo));
      cmd.Parameters.Add(new SqlParameter("@progNo", progNo));
      SqlDataReader drd = cmd.ExecuteReader();
      string result = "null";
      if (drd.HasRows)
      {
        result = convertToJSON(drd, true);
      }
      con.Close();
      Context.Response.Write(result);
    }

    [WebMethod]
    public void isIdUnique(string ecomId)
    {
      SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString);
      con.Open();
      SqlCommand cmd = new SqlCommand();
      cmd.Connection = con;
      cmd.CommandText = "dbo.sp7isIdUnique";
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.Parameters.Add(new SqlParameter("@ecomId", ecomId));
      SqlDataReader drd = cmd.ExecuteReader();
      string result = "null";
      if (drd.HasRows)
      {
        result = convertToJSON(drd, false);
      }
      con.Close();
      Context.Response.Write(result);
    }

    [WebMethod]
    public void learner(string membGuid)
    {
      SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString);
      con.Open();
      SqlCommand cmd = new SqlCommand();
      cmd.Connection = con;
      cmd.CommandText = "dbo.sp7learner";
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.Parameters.Add(new SqlParameter("@membGuid", membGuid));
      SqlDataReader drd = cmd.ExecuteReader();
      string result = "null";
      if (drd.HasRows)
      {
        result = convertToJSON(drd, false);
      }
      con.Close();
      Context.Response.Write(result);
    }

    [WebMethod]
    public void learnerActivity(string custId, string certPrograms, string certPrograms_E)
    {
      SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString);
      con.Open();
      SqlCommand cmd = new SqlCommand();
      cmd.Connection = con;
      cmd.CommandText = "dbo.sp8learnerActivity";
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.Parameters.Add(new SqlParameter("@custId", custId));
      cmd.Parameters.Add(new SqlParameter("@certPrograms", certPrograms));
      cmd.Parameters.Add(new SqlParameter("@certPrograms_E", certPrograms_E));
      SqlDataReader drd = cmd.ExecuteReader();
      string result = "null";
      if (drd.HasRows)
      {
        result = convertToJSON(drd, false);
      }
      con.Close();
      Context.Response.Write(result);
    }

    [WebMethod]
    public void learnerDelete(string membGuid)
    {
      SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString);
      con.Open();
      SqlCommand cmd = new SqlCommand();
      cmd.Connection = con;
      cmd.CommandText = "dbo.sp7learnerDelete";
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.Parameters.Add(new SqlParameter("@membGuid", membGuid));
      SqlDataReader drd = cmd.ExecuteReader();
      string result = "null";
      if (drd.HasRows)
      {
        result = convertToJSON(drd, false);
      }
      con.Close();
      Context.Response.Write(result);
    }

    [WebMethod]
    public void learnerEdit(bool membActive, string membEmail, string membFirstName, string membGuid, string membLastName, int membLevel, string membMemo)
    {
      SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString);
      con.Open();
      SqlCommand cmd = new SqlCommand();
      cmd.Connection = con;
      cmd.CommandText = "dbo.sp7learnerEdit";
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.Parameters.Add(new SqlParameter("@membActive", membActive));
      cmd.Parameters.Add(new SqlParameter("@membEmail", membEmail));
      cmd.Parameters.Add(new SqlParameter("@membFirstName", membFirstName));
      cmd.Parameters.Add(new SqlParameter("@membGuid", membGuid));
      cmd.Parameters.Add(new SqlParameter("@membLastName", membLastName));
      cmd.Parameters.Add(new SqlParameter("@membLevel", membLevel));
      cmd.Parameters.Add(new SqlParameter("@membMemo", membMemo));
      SqlDataReader drd = cmd.ExecuteReader();
      string result = "null";
      if (drd.HasRows)
      {
        result = convertToJSON(drd, false);
      }
      con.Close();
      Context.Response.Write(result);
    }

    [WebMethod]
    public void learnerNew(string custId, bool membActive, string membId, string membEmail, string membFirstName, string membLastName, int membLevel, string membMemo, string membPassword)
    {
      SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString);
      con.Open();
      SqlCommand cmd = new SqlCommand();
      cmd.Connection = con;
      cmd.CommandText = "dbo.sp7learnerNew";
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.Parameters.Add(new SqlParameter("@custId", custId));
      cmd.Parameters.Add(new SqlParameter("@membActive", membActive));
      cmd.Parameters.Add(new SqlParameter("@membId", membId));
      cmd.Parameters.Add(new SqlParameter("@membEmail", membEmail));
      cmd.Parameters.Add(new SqlParameter("@membFirstName", membFirstName));
      cmd.Parameters.Add(new SqlParameter("@membLastName", membLastName));
      cmd.Parameters.Add(new SqlParameter("@membLevel", membLevel));
      cmd.Parameters.Add(new SqlParameter("@membMemo", membMemo));
      cmd.Parameters.Add(new SqlParameter("@membPassword", membPassword));
      SqlDataReader drd = cmd.ExecuteReader();
      string result = "null";
      if (drd.HasRows)
      {
        result = convertToJSON(drd, false);
      }
      con.Close();
      Context.Response.Write(result);
    }

    [WebMethod]
    public void learners(string custId, string membType, string membLastName, int membLevel)
    {
      SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString);
      con.Open();
      SqlCommand cmd = new SqlCommand();
      cmd.Connection = con;
      cmd.CommandText = "dbo.sp7learners";
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.Parameters.Add(new SqlParameter("@custId", custId));
      cmd.Parameters.Add(new SqlParameter("@membType", membType));
      cmd.Parameters.Add(new SqlParameter("@membLastName", membLastName));
      cmd.Parameters.Add(new SqlParameter("@membLevel", membLevel));
      SqlDataReader drd = cmd.ExecuteReader();

      string result = "null";
      if (drd.HasRows) result = convertToJSON(drd, true);
      con.Close();
      Context.Response.Write(result);
    }

    [WebMethod]
    public void member(string membGuid) // get member using membGuidTemp
    {
      SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString);
      con.Open();
      SqlCommand cmd = new SqlCommand();
      cmd.Connection = con;
      cmd.CommandText = "dbo.sp8member";
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.Parameters.Add(new SqlParameter("@membGuid", membGuid));
      SqlDataReader drd = cmd.ExecuteReader();
      string result = "null";
      if (drd.HasRows)
      {
        result = convertToJSON(drd, false);
      }
      con.Close();
      Context.Response.Write(result);
    }

    [WebMethod]
    public void module(string modsId)
    {
      SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString);
      con.Open();
      SqlCommand cmd = new SqlCommand();
      cmd.Connection = con;
      cmd.CommandText = "dbo.sp8module";
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.Parameters.Add(new SqlParameter("@modsId", modsId));
      SqlDataReader drd = cmd.ExecuteReader();
      string result = "null";
      if (drd.HasRows)
      {
        result = convertToJSON(drd, false);
      }
      con.Close();
      Context.Response.Write(result);
    }

    [WebMethod]
    public void modules(string custId, int catlNo, string progId, int membNo)
    {
      SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString);
      con.Open();
      SqlCommand cmd = new SqlCommand();
      cmd.Connection = con;
      cmd.CommandText = "dbo.sp8modules";
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.Parameters.Add(new SqlParameter("@custId", custId));
      cmd.Parameters.Add(new SqlParameter("@catlNo", catlNo));
      cmd.Parameters.Add(new SqlParameter("@progId", progId));
      cmd.Parameters.Add(new SqlParameter("@membNo", membNo));
      SqlDataReader drd = cmd.ExecuteReader();
      string result = "null";
      if (drd.HasRows)
      {
        result = convertToJSON(drd, true);
      }
      con.Close();
      Context.Response.Write(result);
    }

    [WebMethod]
    public void modulesEcommerce(string progId, int membNo)
    {
      SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString);
      con.Open();
      SqlCommand cmd = new SqlCommand();
      cmd.Connection = con;
      cmd.CommandText = "dbo.sp8modulesEcommerce";
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.Parameters.Add(new SqlParameter("@progId", progId));
      cmd.Parameters.Add(new SqlParameter("@membNo", membNo));
      SqlDataReader drd = cmd.ExecuteReader();
      string result = "null";
      if (drd.HasRows)
      {
        result = convertToJSON(drd, true);
      }
      con.Close();
      Context.Response.Write(result);
    }

    [WebMethod]
    public void password(string membGuid, string passwordOld, string passwordNew)
    {
      SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString);
      con.Open();
      SqlCommand cmd = new SqlCommand();
      cmd.Connection = con;
      cmd.CommandText = "dbo.sp8password";
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.Parameters.Add(new SqlParameter("@membGuid", membGuid));
      cmd.Parameters.Add(new SqlParameter("@passwordOld", passwordOld));
      cmd.Parameters.Add(new SqlParameter("@passwordNew", passwordNew));
      SqlDataReader drd = cmd.ExecuteReader();
      string result = "null";
      if (drd.HasRows)
      {
        result = convertToJSON(drd, false);
      }
      con.Close();
      Context.Response.Write(result);
    }

    [WebMethod]
    public void programActivity(string custId, int orderBy)
    {
      SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString);
      con.Open();
      SqlCommand cmd = new SqlCommand();
      cmd.Connection = con;
      cmd.CommandText = "dbo.sp7programActivity";
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.Parameters.Add(new SqlParameter("@custId", custId));
      cmd.Parameters.Add(new SqlParameter("@orderBy", orderBy));
      SqlDataReader drd = cmd.ExecuteReader();
      string result = "null";
      if (drd.HasRows)
      {
        result = convertToJSON(drd, true);
      }
      con.Close();
      Context.Response.Write(result);
    }

    [WebMethod]
    public void programs(string custId, int catlNo, int membNo)
    {
      SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString);
      con.Open();
      SqlCommand cmd = new SqlCommand();
      cmd.Connection = con;
      cmd.CommandText = "dbo.sp8programs";
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.Parameters.Add(new SqlParameter("@custId", custId));
      cmd.Parameters.Add(new SqlParameter("@catlNo", catlNo));
      cmd.Parameters.Add(new SqlParameter("@membNo", membNo));
      SqlDataReader drd = cmd.ExecuteReader();
      string result = "null";
      if (drd.HasRows)
      {
        result = convertToJSON(drd, true);
      }
      con.Close();
      Context.Response.Write(result);
    }

    [WebMethod]
    public void programsEcommerce(string custId, int membNo)
    {
      SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString);
      con.Open();
      SqlCommand cmd = new SqlCommand();
      cmd.Connection = con;
      cmd.CommandText = "dbo.sp8programsEcommerce";
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.Parameters.Add(new SqlParameter("@custId", custId));
      cmd.Parameters.Add(new SqlParameter("@membNo", membNo));
      SqlDataReader drd = cmd.ExecuteReader();
      string result = "null";
      if (drd.HasRows)
      {
        result = convertToJSON(drd, true);
      }
      con.Close();
      Context.Response.Write(result);
    }

    [WebMethod]
    public void programsAssigned(int membNo)
    {
      SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString);
      con.Open();
      SqlCommand cmd = new SqlCommand();
      cmd.Connection = con;
      cmd.CommandText = "dbo.sp8programsAssigned";
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.Parameters.Add(new SqlParameter("@membNo", membNo));
      SqlDataReader drd = cmd.ExecuteReader();
      string result = "null";
      if (drd.HasRows)
      {
        result = convertToJSON(drd, true);
      }
      con.Close();
      Context.Response.Write(result);
    }

    [WebMethod]
    public void samplers(int includeExpired, string sampId)
    {
      SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString);
      con.Open();
      SqlCommand cmd = new SqlCommand();
      cmd.Connection = con;
      cmd.CommandText = "dbo.spSamplers";
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.Parameters.Add(new SqlParameter("@includeExpired", includeExpired));
      cmd.Parameters.Add(new SqlParameter("@sampId", sampId));
      SqlDataReader drd = cmd.ExecuteReader();
      string result = "null";
      if (drd.HasRows)
      {
        result = convertToJSON(drd, true);
      }
      con.Close();
      Context.Response.Write(result);
    }

    [WebMethod]
    public void sampler(int sampNo)
    {
      SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString);
      con.Open();
      SqlCommand cmd = new SqlCommand();
      cmd.Connection = con;
      cmd.CommandText = "dbo.spSampler";
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.Parameters.Add(new SqlParameter("@sampNo", sampNo));
      SqlDataReader drd = cmd.ExecuteReader();
      string result = "null";
      if (drd.HasRows)
      {
        result = convertToJSON(drd, true);
      }
      con.Close();
      Context.Response.Write(result);
    }

    [WebMethod]
    public void samplerUpdate(int sampNo, string sampId, string sampTitle, string sampTitle01, string sampMods01, string sampTitle02, string sampMods02, string sampTitle03, string sampMods03, string sampTitle04, string sampMods04, string sampTitle05, string sampMods05, string sampTitle06, string sampMods06, string sampTitle07, string sampMods07, string sampTitle08, string sampMods08)
    {
      SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString);
      con.Open();
      SqlCommand cmd = new SqlCommand();
      cmd.Connection = con;
      cmd.CommandText = "dbo.spSamplerUpdate";
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.Parameters.Add(new SqlParameter("@sampNo", sampNo));
      cmd.Parameters.Add(new SqlParameter("@sampId", sampId));
      cmd.Parameters.Add(new SqlParameter("@sampTitle", sampTitle));

      cmd.Parameters.Add(new SqlParameter("@sampTitle01", sampTitle01));
      cmd.Parameters.Add(new SqlParameter("@sampMods01", sampMods01));
      cmd.Parameters.Add(new SqlParameter("@sampTitle02", sampTitle02));
      cmd.Parameters.Add(new SqlParameter("@sampMods02", sampMods02));
      cmd.Parameters.Add(new SqlParameter("@sampTitle03", sampTitle03));
      cmd.Parameters.Add(new SqlParameter("@sampMods03", sampMods03));
      cmd.Parameters.Add(new SqlParameter("@sampTitle04", sampTitle04));
      cmd.Parameters.Add(new SqlParameter("@sampMods04", sampMods04));
      cmd.Parameters.Add(new SqlParameter("@sampTitle05", sampTitle05));
      cmd.Parameters.Add(new SqlParameter("@sampMods05", sampMods05));
      cmd.Parameters.Add(new SqlParameter("@sampTitle06", sampTitle06));
      cmd.Parameters.Add(new SqlParameter("@sampMods06", sampMods06));
      cmd.Parameters.Add(new SqlParameter("@sampTitle07", sampTitle07));
      cmd.Parameters.Add(new SqlParameter("@sampMods07", sampMods07));
      cmd.Parameters.Add(new SqlParameter("@sampTitle08", sampTitle08));
      cmd.Parameters.Add(new SqlParameter("@sampMods08", sampMods08));

      SqlDataReader drd = cmd.ExecuteReader();
      string result = "null";
      if (drd.HasRows)
      {
        result = convertToJSON(drd, false);
      }
      con.Close();
      Context.Response.Write(result);
    }

    [WebMethod]
    public void samplerDelete(int sampNo)
    {
      SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString);
      con.Open();
      SqlCommand cmd = new SqlCommand();
      cmd.Connection = con;
      cmd.CommandText = "dbo.spSamplerDelete";
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.Parameters.Add(new SqlParameter("@sampNo", sampNo));
      SqlDataReader drd = cmd.ExecuteReader();
      string result = "null";
      if (drd.HasRows)
      {
        result = convertToJSON(drd, false);
      }
      con.Close();
      Context.Response.Write(result);
    }

    [WebMethod]
    public void samplerModsTitle(string sampModsId)
    {
      SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString);
      con.Open();
      SqlCommand cmd = new SqlCommand();
      cmd.Connection = con;
      cmd.CommandText = "dbo.spSamplerModsTitle";
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.Parameters.Add(new SqlParameter("@sampModsId", sampModsId));
      SqlDataReader drd = cmd.ExecuteReader();
      string result = "null";
      if (drd.HasRows)
      {
        result = convertToJSON(drd, true);
      }
      con.Close();
      Context.Response.Write(result);
    }

    [WebMethod]
    public void samplerMods(string sampMods)
    {
      SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString);
      con.Open();
      SqlCommand cmd = new SqlCommand();
      cmd.Connection = con;
      cmd.CommandText = "dbo.spSamplerMods";
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.Parameters.Add(new SqlParameter("@sampMods", sampMods));
      SqlDataReader drd = cmd.ExecuteReader();
      string result = "null";
      if (drd.HasRows)
      {
        result = convertToJSON(drd, true);
      }
      con.Close();
      Context.Response.Write(result);
    }

    [WebMethod]
    public void samplerNo(string sampId)
    {
      SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString);
      con.Open();
      SqlCommand cmd = new SqlCommand();
      cmd.Connection = con;
      cmd.CommandText = "dbo.spSamplerNo";
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.Parameters.Add(new SqlParameter("@sampId", sampId));
      SqlDataReader drd = cmd.ExecuteReader();
      string result = "null";
      if (drd.HasRows)
      {
        result = convertToJSON(drd, false);
      }
      con.Close();
      Context.Response.Write(result);
    }

    [WebMethod]
    public string samplerLogoUpload()
    {
      // upload sampler logo to common /samplerLogos web folder (typically only one logo is submitted)
      string path = System.Web.Hosting.HostingEnvironment.MapPath("/samplerLogos/");
      string msg = "";
      HttpFileCollection files = HttpContext.Current.Request.Files;
      for (int i = 0; i < files.Count; i++)
      {
        HttpPostedFile file = files[i];
        string fileName = System.IO.Path.GetFileName(file.FileName);
        string complete = path + "\\" + fileName;

        // first wipe out any existing files with same prefix
        string[] fileBits = fileName.Split('.');
        string[] picList = Directory.GetFiles(path, fileBits[0] + ".*");
        foreach (string f in picList)
        {
          File.Delete(f);
        }

        // now save the new logo
        file.SaveAs(complete);
        msg += "File: " + file.FileName + ", Size: " + file.ContentLength + ", Type: " + file.ContentType + ", OK...";

        // update the sampler record with this image (the sampId is left part of the name) - no need to return status
        string sampImage = file.FileName;

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString);
        con.Open();
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "dbo.spSamplerLogoUpload";
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.Add(new SqlParameter("@sampImage", sampImage));
        cmd.ExecuteNonQuery();
        con.Close();

      }
      return ("ok");
    }


    // new sampler app SPs using sp8sampler...


    [WebMethod]
    public void sp8module(string modsId)
    {
      SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString);
      con.Open();
      SqlCommand cmd = new SqlCommand();
      cmd.Connection = con;
      cmd.CommandText = "dbo.sp8module";
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.Parameters.Add(new SqlParameter("@modsId", modsId));
      SqlDataReader drd = cmd.ExecuteReader();
      string result = "null";
      if (drd.HasRows)
      {
        result = convertToJSON(drd, false);
      }
      con.Close();
      Context.Response.Write(result);
    }

    [WebMethod]
    public void sp8sampler(int sampNo)
    {
      SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString);
      con.Open();
      SqlCommand cmd = new SqlCommand();
      cmd.Connection = con;
      cmd.CommandText = "dbo.sp8sampler";
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.Parameters.Add(new SqlParameter("@sampNo", sampNo));
      SqlDataReader drd = cmd.ExecuteReader();
      string result = "null";
      if (drd.HasRows)
      {
        result = convertToJSON(drd, false);
      }
      con.Close();
      Context.Response.Write(result);
    }

    [WebMethod]
    public void sp8samplerId(string sampId)
    {
      SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString);
      con.Open();
      SqlCommand cmd = new SqlCommand();
      cmd.Connection = con;
      cmd.CommandText = "dbo.sp8samplerId";
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.Parameters.Add(new SqlParameter("@sampId", sampId));
      SqlDataReader drd = cmd.ExecuteReader();
      string result = "null";
      if (drd.HasRows)
      {
        result = convertToJSON(drd, false);
      }
      con.Close();
      Context.Response.Write(result);
    }

    [WebMethod]
    public void sp8samplerCont(string sampCont)
    {
      SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString);
      con.Open();
      SqlCommand cmd = new SqlCommand();
      cmd.Connection = con;
      cmd.CommandText = "dbo.sp8samplerCont";
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.Parameters.Add(new SqlParameter("@sampCont", sampCont));
      SqlDataReader drd = cmd.ExecuteReader();
      string result = "null";
      if (drd.HasRows)
      {
        result = convertToJSON(drd, true);
      }
      con.Close();
      Context.Response.Write(result);
    }

    [WebMethod]
    public string sp8samplerLogoUpload()
    {
      // this gets the image via the "file" collection
      HttpFileCollection files = HttpContext.Current.Request.Files;
      HttpPostedFile file = files["file"];
      string fileName = file.FileName;
      int slash = fileName.LastIndexOf('\\');                                                 // IE/Edge fileName includes full URL of where it came from, just get name
      if (slash > 0) fileName = fileName.Substring(slash + 1);

      // get the other parameters from the form collection plus split the filename
      string imageFolder = HttpContext.Current.Request.Form["imageFolder"];
      int dot = fileName.LastIndexOf('.');
      string inName = fileName.Substring(0, dot);
      string inType = fileName.Substring(dot + 1);
      string toName = HttpContext.Current.Request.Form["toName"];
      int maxSize = Int32.Parse(HttpContext.Current.Request.Form["maxSize"]);                // get maximum height or width

      // save uploaded image (will be deleted later)
      fileName = Server.MapPath("/" + imageFolder + "/" + fileName);
      file.SaveAs(fileName);

      // prepare the original filename, a temp version and a final version
      string fileName1 = Server.MapPath("/" + imageFolder + "/" + inName + "." + inType);    // get full file name of uploaded image
      string fileName2 = fileName1.Replace(inName, toName).Replace("." + inType, "_.png");   // modify this filename with the "toName plus "_.png" type (temp)
      string fileName3 = fileName2.Replace("_.png", ".png");                                 // modify this as final filename					

      // retype first fileName from any image format (including png) to png
      Image image1 = Image.FromFile(fileName1);                                              // load uploaded image as a bitmap
      image1.Save(fileName2, System.Drawing.Imaging.ImageFormat.Png);                        // save as the temp filename as png
      image1.Dispose();                                                                      // displose so we can resize			

      // resize second fileName to max n pixels
      using (var image2 = new MagickImage(fileName2))
      {
        MagickGeometry geometry;
        // only reduce size if one side is greater then the maxSize
        if (image2.Width > maxSize || image2.Height > maxSize)
        {
          if (image2.Width >= image2.Height)
          {
            geometry = new MagickGeometry(maxSize, 0);
          }
          else
          {
            geometry = new MagickGeometry(0, maxSize);
          }
          geometry.IgnoreAspectRatio = false;
          image2.Resize(geometry);
        }
        image2.Write(fileName3);
      }

      // delete all but final image file
      File.Delete(fileName1);
      File.Delete(fileName2);

      // update the sampler record showing showing image was uploaded (puts True in the sampImage field)
      SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString);
      con.Open();
      SqlCommand cmd = new SqlCommand();
      cmd.Connection = con;
      cmd.CommandText = "dbo.sp8samplerLogoUpload";
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.Parameters.Add(new SqlParameter("@samplerId", toName));
      cmd.ExecuteNonQuery();
      con.Close();

      return ("ok");
    }

    [WebMethod]
    public void sp8samplerModsTitle(string sampModsId)
    {
      SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString);
      con.Open();
      SqlCommand cmd = new SqlCommand();
      cmd.Connection = con;
      cmd.CommandText = "dbo.sp8samplerModsTitle";
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.Parameters.Add(new SqlParameter("@sampModsId", sampModsId));
      SqlDataReader drd = cmd.ExecuteReader();
      string result = "null";
      if (drd.HasRows)
      {
        result = convertToJSON(drd, true);
      }
      con.Close();
      Context.Response.Write(result);
    }

    [WebMethod]
    public void sp8samplers(int includeExpired, string sampId)
    {
      SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString);
      con.Open();
      SqlCommand cmd = new SqlCommand();
      cmd.Connection = con;
      cmd.CommandText = "dbo.sp8samplers";
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.Parameters.Add(new SqlParameter("@includeExpired", includeExpired));
      cmd.Parameters.Add(new SqlParameter("@sampId", sampId));
      SqlDataReader drd = cmd.ExecuteReader();

      //string samplerId - get status of sampler logo
      //while (drd.Read())
      //{
      //  string samplerId = drd.GetString(1);
      //}


      string result = "null";
      if (drd.HasRows)
      {
        result = convertToJSON(drd, true);
      }
      con.Close();
      Context.Response.Write(result);
    }

    [WebMethod]
    public void sp8samplerSettings(int sampNo)
    {
      SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString);
      con.Open();
      SqlCommand cmd = new SqlCommand();
      cmd.Connection = con;
      cmd.CommandText = "dbo.sp8samplerSettings";
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.Parameters.Add(new SqlParameter("@sampNo", sampNo));
      SqlDataReader drd = cmd.ExecuteReader();
      string result = "null";
      if (drd.HasRows)
      {
        result = convertToJSON(drd, true);
      }
      con.Close();
      Context.Response.Write(result);
    }

    [WebMethod]
    public void sp8samplerSettingsUpd(
      int sampNo, string sampExpires, int sampLimitPages, int sampStartPage,
      int sampEndPage, int sampMaxVisits, int sampMaxMinutes, int sampProduction
      )
    {
      SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString);
      con.Open();
      SqlCommand cmd = new SqlCommand();
      cmd.Connection = con;
      cmd.CommandText = "dbo.sp8samplerSettingsUpd";
      cmd.CommandType = CommandType.StoredProcedure;

      cmd.Parameters.Add(new SqlParameter("@sampNo", sampNo));
      cmd.Parameters.Add(new SqlParameter("@sampExpires", sampExpires));
      cmd.Parameters.Add(new SqlParameter("@sampLimitPages", sampLimitPages));
      cmd.Parameters.Add(new SqlParameter("@sampStartPage", sampStartPage));
      cmd.Parameters.Add(new SqlParameter("@sampEndPage", sampEndPage));
      cmd.Parameters.Add(new SqlParameter("@sampMaxVisits", sampMaxVisits));
      cmd.Parameters.Add(new SqlParameter("@sampMaxMinutes", sampMaxMinutes));
      cmd.Parameters.Add(new SqlParameter("@sampProduction", sampProduction));

      SqlDataReader drd = cmd.ExecuteReader();
      string result = "null";
      if (drd.HasRows)
      {
        result = convertToJSON(drd, true);
      }
      con.Close();
      Context.Response.Write(result);
    }

    [WebMethod]
    public void sp8samplerSignIn(string membAcctId, string membId, string membPwd)
    {
      SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString);
      con.Open();
      SqlCommand cmd = new SqlCommand();
      cmd.Connection = con;
      cmd.CommandText = "dbo.sp8samplerSignIn";
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.Parameters.Add(new SqlParameter("@membAcctId", membAcctId));
      cmd.Parameters.Add(new SqlParameter("@membId", membId));
      cmd.Parameters.Add(new SqlParameter("@membPwd", membPwd));
      SqlDataReader drd = cmd.ExecuteReader();
      string result = "null";
      if (drd.HasRows)
      {
        result = convertToJSON(drd, false);
      }
      con.Close();
      Context.Response.Write(result);
    }

    [WebMethod]
    public void sp8samplerUpdate(int sampNo, string sampId, string sampTitle, string sampTitle01, string sampCont01, string sampTitle02, string sampCont02, string sampTitle03, string sampCont03, string sampTitle04, string sampCont04, string sampTitle05, string sampCont05, string sampTitle06, string sampCont06, string sampTitle07, string sampCont07, string sampTitle08, string sampCont08)
    {
      SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString);
      con.Open();
      SqlCommand cmd = new SqlCommand();
      cmd.Connection = con;
      cmd.CommandText = "dbo.sp8samplerUpdate";
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.Parameters.Add(new SqlParameter("@sampNo", sampNo));
      cmd.Parameters.Add(new SqlParameter("@sampId", sampId));
      cmd.Parameters.Add(new SqlParameter("@sampTitle", sampTitle));

      cmd.Parameters.Add(new SqlParameter("@sampTitle01", sampTitle01));
      cmd.Parameters.Add(new SqlParameter("@sampCont01", sampCont01));
      cmd.Parameters.Add(new SqlParameter("@sampTitle02", sampTitle02));
      cmd.Parameters.Add(new SqlParameter("@sampCont02", sampCont02));
      cmd.Parameters.Add(new SqlParameter("@sampTitle03", sampTitle03));
      cmd.Parameters.Add(new SqlParameter("@sampCont03", sampCont03));
      cmd.Parameters.Add(new SqlParameter("@sampTitle04", sampTitle04));
      cmd.Parameters.Add(new SqlParameter("@sampCont04", sampCont04));
      cmd.Parameters.Add(new SqlParameter("@sampTitle05", sampTitle05));
      cmd.Parameters.Add(new SqlParameter("@sampCont05", sampCont05));
      cmd.Parameters.Add(new SqlParameter("@sampTitle06", sampTitle06));
      cmd.Parameters.Add(new SqlParameter("@sampCont06", sampCont06));
      cmd.Parameters.Add(new SqlParameter("@sampTitle07", sampTitle07));
      cmd.Parameters.Add(new SqlParameter("@sampCont07", sampCont07));
      cmd.Parameters.Add(new SqlParameter("@sampTitle08", sampTitle08));
      cmd.Parameters.Add(new SqlParameter("@sampCont08", sampCont08));

      SqlDataReader drd = cmd.ExecuteReader();
      string result = "null";
      if (drd.HasRows)
      {
        result = convertToJSON(drd, false);
      }
      con.Close();
      Context.Response.Write(result);
    }

    [WebMethod]
    public void sp8tokenSet(int minutes)
    {
      SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString);
      con.Open();
      SqlCommand cmd = new SqlCommand();
      cmd.Connection = con;
      cmd.CommandText = "dbo.sp8tokenSet";
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.Parameters.Add(new SqlParameter("@minutes", minutes));
      SqlDataReader drd = cmd.ExecuteReader();
      string result = "null";
      if (drd.HasRows)
      {
        result = convertToJSON(drd, false);
      }
      con.Close();
      Context.Response.Write(result);
    }


    [WebMethod]
    public void sp5authenticate(string membId)
    {
      SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString);
      con.Open();
      SqlCommand cmd = new SqlCommand();
      cmd.Connection = con;
      cmd.CommandText = "dbo.sp5authenticate";
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.Parameters.Add(new SqlParameter("@membId", membId));
      SqlDataReader drd = cmd.ExecuteReader();
      string result = "null";
      if (drd.HasRows)
      {
        result = convertToJSON(drd, false);
      }
      con.Close();
      Context.Response.Write(result);
    }

    [WebMethod]
    public void sp5findLearner(string cust, string membFirstName, string membLastName)
    {
      SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString);
      con.Open();
      SqlCommand cmd = new SqlCommand();
      cmd.Connection = con;
      cmd.CommandText = "dbo.sp5findLearners";
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.Parameters.Add(new SqlParameter("@cust", cust));
      cmd.Parameters.Add(new SqlParameter("@membFirstName", membFirstName));
      cmd.Parameters.Add(new SqlParameter("@membLastName", membLastName));
      SqlDataReader drd = cmd.ExecuteReader();

      string result = "null";
      if (drd.HasRows) result = convertToJSON(drd, true);
      con.Close();
      Context.Response.Write(result);
    }

    [WebMethod]
    public void sp5learnerProfile(int membNo)
    {
      SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString);
      con.Open();
      SqlCommand cmd = new SqlCommand();
      cmd.Connection = con;
      cmd.CommandText = "dbo.sp5learnerProfile";
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.Parameters.Add(new SqlParameter("@membNo", membNo));
      SqlDataReader drd = cmd.ExecuteReader();
      string result = "null";
      if (drd.HasRows)
      {
        result = convertToJSON(drd, false);
      }
      con.Close();
      Context.Response.Write(result);
    }

    [WebMethod]
    public void sp5learnerPrograms(int membNo)
    {
      SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString);
      con.Open();
      SqlCommand cmd = new SqlCommand();
      cmd.Connection = con;
      cmd.CommandText = "dbo.sp5learnerPrograms";
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.Parameters.Add(new SqlParameter("@membNo", membNo));
      SqlDataReader drd = cmd.ExecuteReader();
      string result = "null";
      if (drd.HasRows)
      {
        result = convertToJSON(drd, true);
      }
      con.Close();
      Context.Response.Write(result);
    }

    [WebMethod]
    public void sp5learnerModules(int membNo, int progNo)
    {
      SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString);
      con.Open();
      SqlCommand cmd = new SqlCommand();
      cmd.Connection = con;
      cmd.CommandText = "dbo.sp5learnerModules";
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.Parameters.Add(new SqlParameter("@membNo", membNo));
      cmd.Parameters.Add(new SqlParameter("@progNo", progNo));
      SqlDataReader drd = cmd.ExecuteReader();
      string result = "null";
      if (drd.HasRows)
      {
        result = convertToJSON(drd, true);
      }
      con.Close();
      Context.Response.Write(result);
    }



    // these are used to determine the size of an excel report (clientside)
    [WebMethod]
    public void ecommerceCount(string cust, string strDate, string endDate)
    { // count the number of ecommerce records for this channel (cust) between these dates
      SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString);
      con.Open();
      SqlCommand cmd = new SqlCommand();
      cmd.Connection = con;
      cmd.CommandText = "dbo.spXecommerceCount";
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.Parameters.Add(new SqlParameter("@cust", cust));
      cmd.Parameters.Add(new SqlParameter("@strDate", strDate));
      cmd.Parameters.Add(new SqlParameter("@endDate", endDate));
      SqlDataReader drd = cmd.ExecuteReader();
      string result = "null";
      if (drd.HasRows)
      {
        result = convertToJSON(drd, false);
      }
      con.Close();
      Context.Response.Write(result);
    }

    [WebMethod]
    public void isGovActivityCount(string strDate, string endDate, string organizations)
    { // count the number of programs for this channel (cust) between these dates
      SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString);
      con.Open();
      SqlCommand cmd = new SqlCommand();
      cmd.Connection = con;
      cmd.CommandText = "dbo.spXisGovActivityCount";
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.Parameters.Add(new SqlParameter("@strDate", strDate));
      cmd.Parameters.Add(new SqlParameter("@endDate", endDate));
      cmd.Parameters.Add(new SqlParameter("@organizations", organizations));
      SqlDataReader drd = cmd.ExecuteReader();
      string result = "null";
      if (drd.HasRows)
      {
        result = convertToJSON(drd, false);
      }
      con.Close();
      Context.Response.Write(result);
    }

    [WebMethod]
    public void isGovActivity(string strDate, string endDate, string organizations, int repType)
    { // count the number of programs for organizations/dates - similiar version on server side for report
      SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString);
      con.Open();
      SqlCommand cmd = new SqlCommand();
      cmd.Connection = con;
      cmd.CommandText = "dbo.spXisGovActivity";
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.Parameters.Add(new SqlParameter("@organizations", organizations));
      cmd.Parameters.Add(new SqlParameter("@strDate", strDate));
      cmd.Parameters.Add(new SqlParameter("@endDate", endDate));
      cmd.Parameters.Add(new SqlParameter("@repType", repType));
      SqlDataReader drd = cmd.ExecuteReader();
      string result = "null";
      if (drd.HasRows)
      {
        result = convertToJSON(drd, false);
      }
      con.Close();
      Context.Response.Write(result);
    }

    [WebMethod]
    public void programActivityDetailsCount(string custId, string strDate, string endDate, string membId)
    { // count the number of programs for this channel (cust) between these dates
      SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString);
      con.Open();
      SqlCommand cmd = new SqlCommand();
      cmd.Connection = con;
      cmd.CommandText = "dbo.spXprogramActivityDetailsCount";
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.Parameters.Add(new SqlParameter("@custId", custId));
      cmd.Parameters.Add(new SqlParameter("@strDate", strDate));
      cmd.Parameters.Add(new SqlParameter("@endDate", endDate));
      cmd.Parameters.Add(new SqlParameter("@membId", membId));
      SqlDataReader drd = cmd.ExecuteReader();
      string result = "null";
      if (drd.HasRows)
      {
        result = convertToJSON(drd, false);
      }
      con.Close();
      Context.Response.Write(result);
    }


    private string convertMsgToJSON(string msg)
    {
      msg = "{\"msg\":\"" + msg + "\"}";
      return msg;
    }

    private string convertToJSON(SqlDataReader reader, bool isArray)
    {
      // this is a general routine used in v8client.asmx.cs and v8clientserver.asmx.cs to render properly formatted JSON
      // the isArray, when true will add [] around the objects, else it will not

      if (reader == null || reader.FieldCount == 0)
      {
        return "null";
      }
      int rowCount = 0;
      StringBuilder sb = new StringBuilder();
      if (isArray) { sb.Append("["); };
      while (reader.Read())
      {
        sb.Append("{");
        for (int i = 0; i < reader.FieldCount; i++)
        {
          sb.Append("\"" + reader.GetName(i) + "\":");
          sb.Append("\"" + reader[i] + "\"");
          sb.Append(i == reader.FieldCount - 1 ? "" : ",");
        }
        sb.Append("},");
        rowCount++;
      }
      if (rowCount > 0)
      {
        int index = sb.ToString().LastIndexOf(",");
        sb.Remove(index, 1);
        if (isArray) { sb.Append("]"); };
      }

      //return Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(sb.ToString()));     
      return sb.ToString();
    }

  }

}