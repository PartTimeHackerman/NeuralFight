public interface IAgent
{
    int getHash();
    ObservationsDTO getObservations();
    void pauseAgent(bool pause);
    void receiveActions(ActionsDTO actions);
}