using SldWorks;
using SwConst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoDimension
{
    class Program
    {
        static void Main(string[] args)
        {
            var swInstance = new SldWorks.SldWorks();

            var model = (ModelDoc2)swInstance.ActiveDoc;
            var drawing = (DrawingDoc)model;

            drawing.InsertModelAnnotations3((int)swImportModelItemsSource_e.swImportModelItemsFromEntireModel, 163840, true, true, true, false);

            // TODO - make this program open blob.L2_cover.SLDASM -> write '0' to the "Is Dimensioned" field
            // of blob.L2_cover.txt -> build the cover assembly to suppress the items -> open the drawing document in SW
            // -> call this program's drawing.InsertModelAnnotations3() -> switch active docs to the assembly ->
            // write '1' to the "Is Dimensioned" field of blob.L2_cover.txt -> save and close that document ->
            // continue with the autoDrawing tasks from other programs
        }
    }
}
