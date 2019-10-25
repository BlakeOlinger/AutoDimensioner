using SldWorks;
using SwConst;
using System;
using System.Collections.Generic;
using System.Threading;

namespace AutoDimension
{
    class Program
    {
        static void Main(string[] args)
        {
            var swInstance = new SldWorks.SldWorks();

            // read from app data to populate the 3 expected paths
            // currently: cover assembly, cover assembly config, cover drawing paths
            var appDataPath = @"C:\Users\bolinger\Documents\SolidWorks Projects\Prefab Blob - Cover Blob\app data\rebuild.txt";
            var appDataLines = System.IO.File.ReadAllLines(appDataPath);

            // make blob.L2_cover.SLDASM the active SW document
            var coverAssemblyFileName = getFileNameFromPath(appDataLines[0]);
            var errors = 0;
            var model = (ModelDoc2)swInstance.ActivateDoc3(coverAssemblyFileName, true,
                (int)swRebuildOnActivation_e.swRebuildActiveDoc, ref errors);

            // write to blob.L2_cover.txt '0' to the field "Is Dimensioned"
            var coverConfigPath = appDataLines[1];
            var coverConfigLines = System.IO.File.ReadAllLines(coverConfigPath);
            var index = 0;
            var stop = false;
            while (!stop)
            {
                var line = coverConfigLines[index];

                if (line.Contains("Is Dimensioned") &&
                    !line.Contains("IIF"))
                {
                    stop = true;
                    var newLine = coverConfigLines[index].Replace("1", "0");

                    coverConfigLines[index] = newLine;
                } else
                {
                    ++index;
                }
            }
            System.IO.File.WriteAllLines(coverConfigPath, coverConfigLines);

            // wait for write
            var delay = 300;
            Thread.Sleep(delay);

            // rebuild to suppress assembly features
            model.ForceRebuild3(false);

            // wait a second
            Thread.Sleep(delay);

            // make blob.cover.SLDDRW the active SW document
            var coverDrawingFileName = getFileNameFromPath(appDataLines[2]);
            model = (ModelDoc2)swInstance.ActivateDoc3(coverDrawingFileName, true,
                (int)swRebuildOnActivation_e.swRebuildActiveDoc, ref errors);
            
            // wait a second
            Thread.Sleep(delay);
            
            // draw annotations to drawing doc
            var drawing = (DrawingDoc)model;
            var annotations = (object[])drawing.InsertModelAnnotations3(
                (int)swImportModelItemsSource_e.swImportModelItemsFromEntireModel,
                (int)swInsertAnnotation_e.swInsertDimensionsMarkedForDrawing,
                true, false, false, false);

            // second dimension - hidden doesn't work
            Thread.Sleep(delay);
            model = (ModelDoc2)swInstance.ActivateDoc3(coverAssemblyFileName, true,
              (int)swRebuildOnActivation_e.swRebuildActiveDoc, ref errors);
            var index2 = 0;
            stop = false;
            while (!stop)
            {
                var line = coverConfigLines[index2];

                if (line.Contains("Is Dimensioned 2") &&
                    !line.Contains("IIF"))
                {
                    stop = true;
                    var newLine = coverConfigLines[index2].Replace("1", "0");

                    coverConfigLines[index2] = newLine;
                }
                else
                {
                    ++index2;
                }
            }
            System.IO.File.WriteAllLines(coverConfigPath, coverConfigLines);
            Thread.Sleep(delay);
            model.ForceRebuild3(false);
            Thread.Sleep(delay);
            swInstance.ActivateDoc3(coverDrawingFileName, true,
                (int)swRebuildOnActivation_e.swRebuildActiveDoc, ref errors);
            Thread.Sleep(delay);
            drawing.InsertModelAnnotations3(
                (int)swImportModelItemsSource_e.swImportModelItemsFromEntireModel,
                (int)swInsertAnnotation_e.swInsertDimensionsMarkedForDrawing,
                true, false, false, false);

            // wait a second
            Thread.Sleep(delay);
            
            // make blob.L2_cover.SLDASM the active SW document
            model = (ModelDoc2)swInstance.ActivateDoc3(coverAssemblyFileName, true,
              (int)swRebuildOnActivation_e.swRebuildActiveDoc, ref errors);

            // write unsuppress to assembly config
            coverConfigLines[index] = coverConfigLines[index].Replace("0", "1");
            coverConfigLines[index2] = coverConfigLines[index2].Replace("0", "1");
            System.IO.File.WriteAllLines(coverConfigPath, coverConfigLines);
            
            // wait a second
            Thread.Sleep(delay);

            // rebuild assembly - necessary to unsuppress drawing
            model.ForceRebuild3(false);

            // wait a second
            Thread.Sleep(delay);

            // final rebuild
            model.ForceRebuild3(false);

            // wait a second
            Thread.Sleep(delay);

            // make blob.cover.SLDDRW the active SW document
            model = (ModelDoc2)swInstance.ActivateDoc3(coverDrawingFileName, true,
                (int)swRebuildOnActivation_e.swRebuildActiveDoc, ref errors);
        }
        private static void waitForInput()
        {
            displayLines(" ... Press Any Key to Continue.");
            Console.ReadLine();
            Console.Clear();
        }
        private static string getFileNameFromPath(string path)
        {
            var pathSegments = path.Split('\\');
            return pathSegments[pathSegments.Length - 1].Trim();
        }
        private static void displayLines(string[] lines)
        {
            foreach (string line in lines)
            {
                Console.WriteLine(line);
            }
        }
        private static void displayLines(string line)
        {
            Console.WriteLine(line);
        }
        private static void displayLines(int line)
        {
            Console.WriteLine(line);
        }
        private static void displayLines(double line)
        {
            Console.WriteLine(line);
        }
        private static void displayLines(double[] lines)
        {
            foreach (double number in lines)
            {
                Console.WriteLine(number);
            }
        }
        private static void displayLines(Dictionary<string, string> dict)
        {
            foreach (string property in dict.Keys)
            {
                Console.WriteLine(property + " : " + dict[property]);
            }
        }
        private static void displayLines(Boolean line)
        {
            Console.WriteLine(line);
        }
    }
}
