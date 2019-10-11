using SldWorks;
using SwConst;
using System.Threading;

namespace AutoDimension
{
    class Program
    {
        static void Main(string[] args)
        {
            var swInstance = new SldWorks.SldWorks();
            
            // open blob.L2_cover.SLDASM
            var coverAssemblyPath = @"C:\Users\bolinger\Documents\SolidWorks Projects\Prefab Blob - Cover Blob\blob - L2\blob.L2_cover.SLDASM";
            var model = (ModelDoc2)swInstance.OpenDoc(coverAssemblyPath, (int)swDocumentTypes_e.swDocASSEMBLY);

            // wait for document to open
            Thread.Sleep(4_000);

            // initial on-open rebuild - top level only: false
            model.ForceRebuild3(false);
            
            // write to blob.L2_cover.txt '0' to the field "Is Dimensioned"
            var coverConfigPath = @"C:\Users\bolinger\Documents\SolidWorks Projects\Prefab Blob - Cover Blob\blob - L2\blob.L2_cover.txt";
            var coverConfigLines = System.IO.File.ReadAllLines(coverConfigPath);
            var isDimensionedLine = coverConfigLines[30];
            var replacedLine = isDimensionedLine.Replace("1", "0");
            coverConfigLines[30] = replacedLine;
            System.IO.File.WriteAllLines(coverConfigPath, coverConfigLines);

            // wait for write
            Thread.Sleep(1_000);
            
            // rebuild to reflect cover config write
            model.ForceRebuild3(false);
            
            // wait for a few seconds
            Thread.Sleep(2_000);

            // open drawing
            var coverDrawingPath = @"C:\Users\bolinger\Documents\SolidWorks Projects\Prefab Blob - Cover Blob\base blob - L1\blob.cover.SLDDRW";
            var drawingDocument = (ModelDoc2)swInstance.OpenDoc(coverDrawingPath, (int)swDocumentTypes_e.swDocDRAWING);

            // wait a few seconds
            Thread.Sleep(3_000);

            // draw dimensions
            model = (ModelDoc2)swInstance.ActiveDoc;
            var drawing = (DrawingDoc)model;
            drawing.InsertModelAnnotations3((int)swImportModelItemsSource_e.swImportModelItemsFromEntireModel, 163840, true, true, true, false);

            // wait a few seconds
            Thread.Sleep(2_000);

            // switch to assembly document
            var frame = (Frame)swInstance.Frame();
            var windows = (object[])frame.ModelWindows;
            var assemblyWindow = (ModelWindow)windows[1];
            frame.ShowModelWindow(assemblyWindow);
            
            // write unsuppress to assembly config
            coverConfigLines = System.IO.File.ReadAllLines(coverConfigPath);
            isDimensionedLine = coverConfigLines[30];
            replacedLine = isDimensionedLine.Replace("0", "1");
            coverConfigLines[30] = replacedLine;
            System.IO.File.WriteAllLines(coverConfigPath, coverConfigLines);

            // wait a few seconds
            Thread.Sleep(2_000);

            // rebuild to unsuppress assembly - necessary to unsuppress open drawing
            model = (ModelDoc2)swInstance.ActiveDoc;
            model.ForceRebuild3(false);
            Thread.Sleep(2_000);
            model.ForceRebuild3(false);

            // wait a few seconds
            Thread.Sleep(2_000);

            // close assembly file
            swInstance.CloseDoc(coverAssemblyPath);
        }
    }
}
