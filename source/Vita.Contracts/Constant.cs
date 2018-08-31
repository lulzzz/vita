using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace Vita.Contracts
{
  public static class Constant
  {
      public static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["Vita"].ConnectionString;
      //public static readonly string ConnectionString = "Server=localhost,1533;Database=Vita;User ID=sa;Password=Vita123!@#;";

    public const string WaTimeZone = "W. Australia Standard Time";
    public const string ArchiveMessage = "[Archived By Website] ";
    public const string Unknown = "unknown";

    public static class MimeTypeHeader
    {
      public const string Pdf = "application/pdf";
    }

    public static class ApiKey
    {
      public const string LogzIo = "wkLyWRqeLzKkffrmDFvSVsnBHxyhALqV";
      public const string GoogleApiFairGo = "AIzaSyCzKbHNK4I0b73ZJgzrmBthAV_bzmSKEHw";
      public const string GoogleApiLendZen = "AIzaSyBP0cfI6d4pgYhS1LbACIwTDaTmPqkG0Tc";
      public const string GoogleApiKey = "AIzaSyAqAvDj93Whwn1semONV65Tb6ReIlpOQr0";
    }

    public static class CosmosDbCollections
    {
      public const string Companies = "Companies";
      public const string KeywordClassifiers = "KeywordClassifiers";
      public const string Localities = "Localities";
      public const string Charges = "Charges";
    }

    public static class Folders
    {
      public static readonly string ShareReference = "vita";
      public static readonly string Templates = "Templates";
      public static readonly string Documents = "Documents";
      public static readonly string Exports = "Exports";
      public static readonly string Tests = "Tests";
      public static readonly string Reports = "Reports";

      public static string BuildLogoImagesPath(string appdata)
      {
        return Path.Combine(appdata, ShareReference);
      }

      public static string BuildTemplatesPath(string appdata)
      {
        return Path.Combine(appdata, ShareReference, Templates);
      }

      public static string BuildDocumentsPath(string appdata)
      {
        return Path.Combine(appdata, ShareReference, Documents);
      }

      public static string BuildExportsPath(string appdata)
      {
        return Path.Combine(appdata, ShareReference, Exports);
      }

      public static string BuildReportsPath(string appdata)
      {
        return Path.Combine(appdata, ShareReference, Reports);
      }

      public static string AzureFolderDocumentsPath()
      {
        return $@"{ShareReference}/{Documents}";
      }
    }

    public static class Math
    {
      // Confirmed with Rebecca, uses these conversion values
      public const double WeeksPerMonth = 4.33;

      public const double FornightsPerMonth = 2.17;

      //https://en.wikipedia.org/wiki/Year
      public static double NumDaysPerYear = 365.2425;
      public static double NumMonthsPerYear = 12;
      public static double NumWeeksPerYear = 52;

      public static double NumFortnightsPerYear = NumWeeksPerYear / 2;

      //
      public static double NumDaysPerMonth = NumDaysPerYear / NumMonthsPerYear; // = 30.44...
      public static double NumDaysPerWeek = NumDaysPerYear / NumWeeksPerYear; // = 7.02...
      public static double NumDaysPerFortnight = NumDaysPerYear / NumFortnightsPerYear; // = 14.05...
    }

    public static class Cache
    {
      public const string BankStatementsInstitutionsList = "BanksStatements.Institutions";
    }

    //TODO move to Fasti.FinPowerService.Constant ? The FinPowerFacade project also has its own constant class
    public static class DecisionCard
    {
      public static readonly string CreditAssesmentDecisionCard = "DC.CRASSES";
      public static readonly string BankStatementsDecisionCard = "DC.BAE";
      public static readonly string LoanApplicationDecisionCard = "DC.APP";
      public static readonly string FgfScoreCard = "DC.SCORE";
      public static readonly string LaccScoreCard = "DC.LACC";
    }

    public static class OrigWorkflow
    {
      public const string BankStatementReceivedStepId = "REC.BS";

      public static class BankStatementReceivedBy
      {
        public const string Electronic = "bs.com.au";
        public const string Upload = "Attachment";
      }

      public static class BankStatementReceivedParameters
      {
        public const string FileLocation = "AttachmentLocation";
        public const string LogPk = "AttachmentLogPk";
      }
    }

    public static class HangfireJobQueue
    {
      public const string Default = "default"; // dont use --> eventflow default
      public const string Web = "web"; // webapi site
      public const string Background = "background"; //webadmin site
    }

    public static class AccountNotes
    {
      public const string NewClient = "New Client";
      public const string ExistingClient = "Existing Client";
    }

    public static class InformationList
    {
      public static readonly string LoanTermLookUpListId = "OFFERAMT";
    }

    public static class Rule
    {
      public static readonly int MinimumCreditCheckAge = 16;
    }

    public static class Client
    {
      /// <summary>
      ///   ClientIds that is built in to system uses. Not actual loan application clients.
      /// </summary>
      public static readonly string[] SystemClientIds = {"U", "I", "COSL", "FOS", "FGF", "MPL", "PLB"};

      /// <summary>
      ///   I - Individual
      /// </summary>
      public static readonly string ClientTypeId = "I";

      /// <summary>
      ///   G - General
      /// </summary>
      public static readonly string ClientGroupId = "G";

      public static readonly string ClientManagerUserId = "Admin";

      /// <summary>
      ///   R - Resident
      /// </summary>
      public static readonly string TaxCategoryId = "R";
    }

    public static class ContactMethod
    {
      public static readonly string EmailMethodId = "EMAIL";
      public static readonly string MobileMethodId = "MOB";
      public static readonly string AddressMethodId = "AD";
      public static readonly string WorkPhone = "PW";
      public static readonly string WebPage = "WEB";
      public static readonly string Phone = "PH";
      public static readonly string LandLord = "LLORD";

      public static class Referee
      {
        public static readonly string Partner = "RELP";
        public static readonly string Parent = "RELN";
        public static readonly string Sibling = "RELS";
        public static readonly string Relative = "RELO";
        public static readonly string Friend = "REF";
        public static readonly string Employer = "REFE";
        public static readonly string Other = "REFU";
        public static readonly string Colleague = "COLG";
      }

      public static class Notes
      {
        public const string SecondaryEmail = "Secondary Email";
        public const string HomePhone = "Home Phone";
      }
    }

    public static class PaymentTypeId
    {
      public static readonly string BankAccount = "BankAccount";
    }

    public static class Security
    {
      public static readonly string MotorVehicleTypeId = "CNMV";
    }

    public static class FluentError
    {
      public static readonly string TooLong =
        "FinPower only accept an {PropertyName} upto {0} chars, but you are supplying {PropertyValue}";
    }

    public static class LengthLimit
    {
      public static readonly int ContactValue = 255;
      public static readonly int ExternalId = 40;
    }

    public class Script
    {
      public static readonly Script CreditCheck = new Script("WSVEDA");
      public static readonly Script SmsEsign = new Script("WSSMSCODE");
      public static readonly Script CapacityToPay = new Script("SF.CAPPAY");
      public static readonly Script BankStatementRequest = new Script("SF.EXECBS");
      public static readonly Script ReEsignContractContinueOriginationsWorkflow = new Script("SF.REVCON");
      public static readonly Script UpdateMonitorCategory = new Script("SF.MONCAT");

      public static readonly Script MissedPayments =
        new Script(MissedPaymentsName, "Find Missed payments and trigger according workflow");

      public static readonly Script AccountProcesses = new Script(AccountProcessesName, "Overnight account processing");

      public static readonly Script AbandonmentProcess =
        new Script(AbandonmentProcessName, "Auto send reminder for abandoned applications");

      public static readonly Script PaymentReminder = new Script(PaymentReminderSms, "Payment reminder sms");

      public const string MissedPaymentsName = "ST.MISSPAY";
      public const string AccountProcessesName = "ST.ACCPROC";
      public const string AbandonmentProcessName = "ST.ABAND";
      public const string EmailMonthlyStatement = "ST.STATE";
      public const string ServicingEmails = "ST.EMAIL";
      public const string PaymentReminderSms = "ST.SMS";


      public string Value { get; }

      public string Description { get; set; }

      public Script(string value)
      {
        Value = value;
      }

      private Script(string value, string description)
      {
        Value = value;
        Description = description;
      }
    }

    public static class Document
    {
      public const string LaccTemplate = "LACC.CON";
      public const string MaccTemplate = "MACC.CON";
      public const string SaccTemplate = "SACC.CON";
      public const string PaymentPlan = "SCH.REPWEB";
      public const string EsignSms = "ESIGN.SMS";
      public static string[] PaymentReminderSms = {"PDUE.SMS", "1PDUE.SMS", "NEWP.SMS", "FINALP.SMS", "FINP.BC.SM"};

      public static string[] DefaultListings =
        {"COLL12A.SM", "COLL12A.E", "COLL6Q.SMS", "COLL6Q.E", "COLLS21D.S", "COLLS21D.E"};
    }

    public static class Fasti
    {
      public const string UserId = "FGFWEBAPI";
    }

    public static class ClientUserData
    {
      public const string IsMate = "IsMate";
      public const string CcCount = "CCCount";
      public const string CcPayments = "CCPayments";
      public const string PartnerRegIncome = "PartnerRegIncome";
      public const string WorkCompRev = "WorkCompRev";
      public static string LivingSituation = "LivingSituation";
    }

    public static class AccountUserData
    {
      public const int PreliminaryFgfScoreIndex = 6;
      public const string PreliminaryFgfScore = "PreliminaryFgfScore";
      public const string CapToPayAmount = "CapToPayAmount";

      public const string CustomerCategory = "CustomerCategory";
      public const string AvailableCash = "AvailableCash";
      public const string AvailableCashFrequency = "AvailableCashFrequency";
      public const string NextPaymentDate = "NextPaymentDate";
      public const string EsignCode = "ESignCode";
      public const string CanOfferSecurity = "SecurityPossible";
      public const string CanProvideVehicleDetail = "SecurityVehicleDetailsAvailable";

      public const string ContractSignedDate = "ContractSignedDate";
      public const string RevisedContractDate = "RevisedContractDate"; // see finpower scripts to match these
      public const string RevisedContractTime = "RevisedContractTime";

      public const int OrigRequestAmtIndex = 1;
      public static string OrigRequestAmt = "OrigRequestAmt";
      public static string OrigLoanType = "OrigLoanType";

      public static string ForseableChanges = "ForseableChanges";
      public static string ForseableChangesExplain = "ForseableChangesExplain";

      public static string MockVedaScore = "MockVedaScore";
      public static string MockBsIsDone = "MockBsIsDone";

      public static string LaccFinalScore = "FinalLACCScore";
      public static string LaccInitialScore = "InitialLACCScore";
      public static string LaccPreliminaryScore = "PreliminaryLACCScore";

      public static class Agreement
      {
        public static class Dvs
        {
          public const string HasAgreed = "DvsConsent";
          public const string Date = "DVSDate";
          public const string Time = "DVSTime";
        }

        public static class Privacy
        {
          public const string HasAgreed = "PrivacyConsent";
          public const string Date = "PrivacyDate";
          public const string Time = "PrivacyTime";
        }

        public static class Referral
        {
          public const string HasAgreed = "ReferralConsent";
          public const string Date = "ReferralDate";
          public const string Time = "ReferralTime";
        }

        public static class BankStatements
        {
          public const string HasAgreed = "BankStatementsConsent";
          public const string Date = "BankStatementsDate";
          public const string Time = "BankStatementsTime";
        }
      }

      public static class WebInfo
      {
        public const int IpAddressIndex = 9;
        public const string IpAddress = "IPAddress";
        public const string Browser = "Browser";
        public const string UrlReferrer = "UrlReferrer";
        public const string QueryString = "QueryString";
      }
    }

    public static class ServiceTypes
    {
      public const string CreditBureau = "CreditBureau";
    }

    public static class Services
    {
      public const string BankStatements = "BankStatements";
      public const string VedaScoreApply = "CreditEnquiry.VedaConnectAU.VedaScoreApply";
    }

    public static class WorkflowTypes
    {
      public const string Origination = "ORIG";
      public const string DefaultListing = "DEFAULT";
    }
  }
}