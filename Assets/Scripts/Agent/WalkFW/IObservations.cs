using System.Collections.Generic;

internal interface IObservations
{

    List<float> getObservations();
    int getObsSize();
    void logNamedObs();
}