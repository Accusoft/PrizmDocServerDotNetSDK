namespace Accusoft.PrizmDocServer.Conversion
{
  public class TiffDestinationOptions
  {
    /// <summary>
    /// When <see langword="true"/>, produce single-page TIFF files, one file
    /// for each page of content (instead of a single TIFF with multiple pages).
    /// Default is <see langword="false"/>.
    /// </summary>
    public bool ForceOneFilePerPage { get; set; }

    /// <summary>
    /// Maximum pixel width of the output TIFF(s), expressed as a CSS-style
    /// string, e.g. "800px". When specified, the output image is guaranteed to
    /// never be wider than the specified value and its aspect ratio will be
    /// preserved. This is useful if you need all of your output images to fit
    /// within a single column.
    /// </summary>
    public string MaxWidth { get; set; }

    /// <summary>
    /// Maximum pixel height of the output TIFF(s), expressed as a CSS-style
    /// string, e.g. "600px". When specified, the output image is guaranteed to
    /// never be taller than the specified value and its aspect ratio will be
    /// preserved. This is useful if you need all of your output images to fit
    /// within a single row.
    /// </summary>
    public string MaxHeight { get; set; }
  }
}