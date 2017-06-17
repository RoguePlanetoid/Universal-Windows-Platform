using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Data.Pdf;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml.Media.Imaging;

public class Library
{
    private PdfDocument document = null;

    public void Show(string content, string title)
    {
        IAsyncOperation<IUICommand> command = new MessageDialog(content, title).ShowAsync();
    }

    public async Task<uint> OpenAsync()
    {
        uint pages = 0;
        try
        {
            document = null;
            FileOpenPicker picker = new FileOpenPicker()
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };
            picker.FileTypeFilter.Add(".pdf");
            StorageFile open = await picker.PickSingleFileAsync();
            if (open != null)
            {
                document = await PdfDocument.LoadFromFileAsync(open);
            }
            if (document != null)
            {
                if (document.IsPasswordProtected)
                {
                    Show("Password Protected PDF Document", "PdfView App");
                }
                pages = document.PageCount;
            }
        }
        catch (Exception ex)
        {
            if (ex.HResult == unchecked((int)0x80004005))
            {
                Show("Invalid PDF Document", "PdfView App");
            }
            else
            {
                Show(ex.Message, "PdfView App");
            }
        }
        return pages;
    }

    public async Task<BitmapImage> ViewAsync(uint number)
    {
        BitmapImage source = new BitmapImage();
        if (!(number < 1 || number > document.PageCount))
        {
            uint index = number - 1;
            using (PdfPage page = document.GetPage(index))
            {
                using (InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream())
                {
                    await page.RenderToStreamAsync(stream);
                    await source.SetSourceAsync(stream);
                }
            }
        }
        return source;
    }

    public List<int> Numbers(int total)
    {
        return Enumerable.Range(1, total).ToList();
    }
}