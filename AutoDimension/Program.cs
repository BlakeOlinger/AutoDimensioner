using SldWorks;
using SwConst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutoDimension
{
    class Program
    {
        static void Main(string[] args)
        {
            var swInstance = new SldWorks.SldWorks();
            
            // open blob.L2_cover.SLDASM
            var model = (ModelDoc2)swInstance.OpenDoc("C:\\Users\\bolinger\\Documents\\SolidWorks Projects\\Prefab Blob - Cover Blob\\blob - L2\\blob.L2_cover.SLDASM", (int)swDocumentTypes_e.swDocASSEMBLY);

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
            // TODO - finish the unsuppres the cover assembly and save step

            // TODO - make this program open blob.L2_cover.SLDASM -> write '0' to the "Is Dimensioned" field
            // of blob.L2_cover.txt -> build the cover assembly to suppress the items -> open the drawing document in SW
            // -> call this program's drawing.InsertModelAnnotations3() -> switch active docs to the assembly ->
            // write '1' to the "Is Dimensioned" field of blob.L2_cover.txt -> save and close that document ->
            // continue with the autoDrawing tasks from other programs
        }
    }
}
