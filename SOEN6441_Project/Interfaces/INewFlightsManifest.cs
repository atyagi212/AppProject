using System;
using Microsoft.AspNetCore.Hosting.Server;

namespace SOEN6441_Project.Interfaces
{
    public interface INewFlightsManifest
    {
        void addObserver(IDatabaseSubscriber observer);

        void removeObserver(IDatabaseSubscriber observer);

        void notifyObservers();
    }
}

