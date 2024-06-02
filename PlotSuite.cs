using ScottPlot;
using Tools;
namespace simpleHttpServer;

public static class PlotSuite
{

    //all file paths start from the root Plots/ directory, you cant use absolute file paths with this method
    public static void plotToFile(float[] X, float[] Y, string path)
    {
        Plot plot = new();

        plot.Add.Scatter(X, Y);

        plot.SavePng(projectFileLoader.pathToFile("Plots/" + path), 1024, 1024);
    }

}
