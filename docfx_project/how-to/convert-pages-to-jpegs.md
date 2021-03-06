# How to Convert the Pages of a File to JPEG Images

This guide explains how to convert the pages of a file to JPEG images.

First, create a [PrizmDocServerClient]:

```csharp
var prizmDocServer = new PrizmDocServerClient(/* your connection info */);
```

Then, call [ConvertAsync] to take a local file, such as
`"project-proposal.docx"`, and have PrizmDoc Server convert each page of the
document to a JPEG:

```csharp
IEnumerable<ConversionResult> results = await prizmDocServer.ConvertAsync("project-proposal.docx", DestinationFileFormat.Jpeg);
```

This will upload the file to PrizmDoc Server, ask PrizmDoc Server to convert the
pages to JPEGs, and then return once the conversion is complete.

The returned results are just _metadata_ about the output; the actual output
files have not been downloaded yet. To actually download the results from
PrizmDoc Server, iterate through each of the results and call
`SaveAsync` on each result:

```csharp
for (var i=0; i < results.Count(); i++)
{
    await results[i].RemoteWorkFile.SaveAsync($"page-{i+1}.jpg");
}
```

Here is a complete example:

```csharp
using System;
using System.Linq;
using System.Threading.Tasks;
using Accusoft.PrizmDocServer;

namespace Demos
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        static async Task MainAsync()
        {
            var prizmDocServer = new PrizmDocServerClient(/* your connection info */);

            // Take a DOCX file and convert each of its pages to a JPEG.
            IEnumerable<ConversionResult> results = await prizmDocServer.ConvertAsync("project-proposal.docx", DestinationFileFormat.Jpeg);

            // Save each result to a JPEG file.
            for (var i=0; i < results.Count(); i++)
            {
                await results.ElementAt(i).RemoteWorkFile.SaveAsync($"page-{i+1}.jpg");
            }
        }
    }
}
```

There are additional overloads of [ConvertAsync] which provide more
flexibility. See the [PrizmDocServerClient] API reference for more information.

[PrizmDocServerClient]: xref:Accusoft.PrizmDocServer.PrizmDocServerClient
[ConvertAsync]: xref:Accusoft.PrizmDocServer.PrizmDocServerClient.ConvertAsync(System.String,Accusoft.PrizmDocServer.Conversion.DestinationFileFormat)
