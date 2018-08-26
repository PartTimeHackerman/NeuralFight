using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TensorFlow;
using UnityEngine;

class InternalModel
{
    private TextAsset graphModel;
    private IObservations observations;
    private IActions actions;
    private TFGraph graph;
    private TFSession session;
    private TFSession.Runner runner;

    public InternalModel(string graphName, IObservations observations, IActions actions)
    {
        this.observations = observations;
        this.actions = actions;
        graphModel = Resources.Load(graphName) as TextAsset;
        graph = new TFGraph();
        graph.Import(graphModel.bytes);
        session = new TFSession(graph);
        
        
    }

    public void act(List<float> obs)
    {
        runner = session.GetRunner();
        float[] obsArr = obs.ToArray();
        //Debug.Log(String.Join(" ",new List<float>(obsArr).ConvertAll(i => i.ToString()).ToArray()));
        //observations.logNamedObs();
        float[,] obsMat = Make2DArray(obsArr, 1, obsArr.Length);
        runner.AddInput(graph["policy/obs"][0], obsMat);
        runner.Fetch(graph["policy/action"][0]);
        float[,] recurrent_tensor = runner.Run()[0].GetValue() as float[,];
        List<float> act = recurrent_tensor.Cast<float>().Select(c => c).ToList();
        actions.applyActions(act);
        
    }

    private static T[,] Make2DArray<T>(T[] input, int height, int width)
    {
        T[,] output = new T[height, width];
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                output[i, j] = input[i * width + j];
            }
        }
        return output;
    }


}
