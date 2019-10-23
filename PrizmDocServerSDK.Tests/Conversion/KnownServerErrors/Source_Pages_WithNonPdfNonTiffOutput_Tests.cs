using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Accusoft.PrizmDocServer.Tests;
using Accusoft.PrizmDocServer.Exceptions;

namespace Accusoft.PrizmDocServer.Conversion.KnownServerErrors.Tests
{
  [TestClass]
  public class Source_Pages_WithNonPdfNonTiffOutput_Tests
  {
    [TestMethod]
    public async Task When_attempting_to_convert_only_page_1_to_PNG()
    {
      var context = Util.CreateContext();

      // In this case, the REST API actually returns 200 and begins the
      // conversion. However, it does not actually honor the source "pages"
      // value. To avoid a breaking change for the SDK consumer, the SDK ensures
      // that the "pages" value was not ignored. If it was, it throws this
      // artificial error, preventing the caller from getting unexpected
      // results.
      await UtilAssert.ThrowsExceptionWithMessageAsync<RestApiErrorException>(async () =>
      {
        await context.ConvertAsync(new SourceDocument("documents/example.docx", pages: "1"), DestinationFileFormat.Png);
      }, $"Remote server does not support taking only specific pages of a source document when the destination type is PNG.");
    }
  }
}