using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using WireMock.Server;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using Accusoft.PrizmDocServer.Exceptions;
using Accusoft.PrizmDocServer.Tests;

namespace Accusoft.PrizmDocServer.Conversion.KnownServerErrors.Tests
{
  [TestClass]
  public class Ocr_BadLanguage_Tests
  {
    static PrizmDocServerClient prizmDocServer;
    static FluentMockServer mockServer;

    [ClassInitialize]
    public static void BeforeAll(TestContext context)
    {
      mockServer = FluentMockServer.Start();
      prizmDocServer = new PrizmDocServerClient("http://localhost:" + mockServer.Ports.First());
    }

    [ClassCleanup]
    public static void AfterAll()
    {
      mockServer.Stop();
      mockServer.Dispose();
    }

    [TestInitialize]
    public void BeforeEach()
    {
      mockServer.Reset();
    }

    [TestMethod]
    public async Task When_server_only_supports_one_OCR_language()
    {
      mockServer
        .Given(Request.Create().WithPath("/v2/contentConverters").UsingPost())
        .RespondWith(Response.Create()
          .WithStatusCode(480)
          .WithHeader("Content-Type", "application/json")
          .WithBody("{\"errorCode\":\"InvalidInput\",\"errorDetails\":{\"in\":\"body\",\"at\":\"input.dest.pdfOptions.ocr.language\",\"expected\":{\"enum\": [\"english\"]}}}"));

      var context = prizmDocServer.CreateProcessingContext();

      var dummyInput = new SourceDocument(new RemoteWorkFile(null, null, null, null));

      await UtilAssert.ThrowsExceptionWithMessageAsync<RestApiErrorException>(async () =>
      {
        await context.ConvertAsync(dummyInput, new DestinationOptions(DestinationFileFormat.Pdf)
        {
          PdfOptions = new PdfDestinationOptions
          {
            Ocr = new OcrOptions
            {
              Language = "hylian"
            }
          }
        });
      }, "Unsupported OCR language \"hylian\". The remote server only supports the following OCR languages: \"english\".");
    }

    [TestMethod]
    public async Task When_server_supports_two_OCR_languages()
    {
      mockServer
        .Given(Request.Create().WithPath("/v2/contentConverters").UsingPost())
        .RespondWith(Response.Create()
          .WithStatusCode(480)
          .WithHeader("Content-Type", "application/json")
          .WithBody("{\"errorCode\":\"InvalidInput\",\"errorDetails\":{\"in\":\"body\",\"at\":\"input.dest.pdfOptions.ocr.language\",\"expected\":{\"enum\":[\"english\",\"russian\"]}}}"));

      var context = prizmDocServer.CreateProcessingContext();

      var dummyInput = new SourceDocument(new RemoteWorkFile(null, null, null, null));

      await UtilAssert.ThrowsExceptionWithMessageAsync<RestApiErrorException>(async () =>
      {
        await context.ConvertAsync(dummyInput, new DestinationOptions(DestinationFileFormat.Pdf)
        {
          PdfOptions = new PdfDestinationOptions
          {
            Ocr = new OcrOptions
            {
              Language = "hylian"
            }
          }
        });
      }, "Unsupported OCR language \"hylian\". The remote server only supports the following OCR languages: \"english\", \"russian\".");
    }

    [TestMethod]
    public async Task When_server_supports_three_OCR_languages()
    {
      mockServer
        .Given(Request.Create().WithPath("/v2/contentConverters").UsingPost())
        .RespondWith(Response.Create()
          .WithStatusCode(480)
          .WithHeader("Content-Type", "application/json")
          .WithBody("{\"errorCode\":\"InvalidInput\",\"errorDetails\":{\"in\":\"body\",\"at\":\"input.dest.pdfOptions.ocr.language\",\"expected\":{\"enum\":[\"english\",\"greek\",\"hebrew\"]}}}"));

      var context = prizmDocServer.CreateProcessingContext();

      var dummyInput = new SourceDocument(new RemoteWorkFile(null, null, null, null));

      await UtilAssert.ThrowsExceptionWithMessageAsync<RestApiErrorException>(async () =>
      {
        await context.ConvertAsync(dummyInput, new DestinationOptions(DestinationFileFormat.Pdf)
        {
          PdfOptions = new PdfDestinationOptions
          {
            Ocr = new OcrOptions
            {
              Language = "hylian"
            }
          }
        });
      }, "Unsupported OCR language \"hylian\". The remote server only supports the following OCR languages: \"english\", \"greek\", \"hebrew\".");
    }
  }
}