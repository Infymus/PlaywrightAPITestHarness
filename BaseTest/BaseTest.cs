using Microsoft.Extensions.Configuration;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using System.Diagnostics;
using VyneSDETTakeHome.DataBase;


namespace VyneSDETTakeHome.TestMethods
{

   /// <summary>
   /// This base test class inherits from Playwright's PageTest.
   /// This class allows bringing different configuration settings from .appsettings. 
   /// It can be modified to use User Secrets, and run in a CI/CD pipeline.
   /// If run in a pipeline, modifications to appsettings.json should include ##variables## and .runsettings modified.
   /// </summary>
   public class BaseTest : PageTest
   {
      // URL and Connection Strings
      public string? InitialUrl;
      public static string? ConnectionString;

      // Target Base URLs
      public static string? JsonPlaceholderBaseUrl;

      // Database
      public static dataBaseQuery? DBQuery;

      // ######### Setup and TearDown #####################################################################

      [OneTimeSetUp]
      public void GlobalSetup()
      {
         // Placeholder in case we want anything that should be set up once per test run.
      }

      /// <summary>
      /// Sets up each test, grabs configuration data from appsettings.json.
      /// </summary>
      [SetUp]
      public void SetupEachTest()
      {
         // SetupEachTest()
         DebugOutput("SetupEachTest()");

         // Configuration Setup
         DebugOutput("ConfigurationBuilder()");
         var configuration = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddEnvironmentVariables()
             .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
             .AddUserSecrets<BaseTest>()
             .Build();

         // Connection Strings
         ConnectionString = configuration["ConnectionStrings:DBConnectionString"];

         // Target URLs
         JsonPlaceholderBaseUrl = configuration["TargetUrls:JsonPlaceholderBaseUrl"];

         // Set default test URL
         InitialUrl = JsonPlaceholderBaseUrl;

         
         // Debug Outputs for Verification
         DebugOutput($"ConnectionString = {ConnectionString}");
         DebugOutput($"JsonPlaceholderBaseUrl = {JsonPlaceholderBaseUrl}");
      }

      [TearDown]
      public void TearDown()
      {
         DebugOutput("TearDown()");
      }

      /// <summary>
      /// Adds to the Console for easy Debugging, Logging & Test Results to Azure Devops
      /// </summary>
      /// <param name="inDebugData"></param>
      public static void DebugOutput(string inDebugData)
      {
         DateTime dateTime = DateTime.Now;
         string formattedDate = dateTime.ToString("MM-dd-yyyy @ hh:mm:ss tt");
         Debug.WriteLine($"{formattedDate} : {inDebugData}");
         TestContext.WriteLine($"{formattedDate} : {inDebugData}");
      }

      /// <summary>
      /// This just writes out a line separater to make it easier to read the debug output
      /// </summary>
      public static void DebugOutputSep()
      {
         DebugOutput($"{new string('=', 60)}");
      }

   }
}
