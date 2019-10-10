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

        }
    }
}
