using System.Collections.Generic;

internal interface IObservations
{

    List<float> GetObservations();
    int getObsSize();
    void logNamedObs();
}