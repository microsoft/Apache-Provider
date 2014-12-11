//-----------------------------------------------------------------------
// <copyright file="ProgressHelper.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.ClientTasks
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Threading;

    /// <summary>
    /// Helper class for the showing/cancelling progress
    /// </summary>
    public class ProgressHelper : IDisposable
    {
        private ProgressChangedEventHandler progressChangedEvent;

        private RunWorkerCompletedEventHandler runWorkerCompletedEvent;

        private DoWorkDelegate doWorkDelegate;

        private ProgressChangedDelegate registedProgressChangedDeletgates;

        private WorkCompletedDelegate registedWorkCompletedDelegates;

        public ProgressHelper(DoWorkDelegate theDelegate)
        {
            this.doWorkDelegate = theDelegate;
            worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += new DoWorkEventHandler(theDelegate);
            CancellationTokenSourceObject = new CancellationTokenSource();
        }

        public ProgressHelper(ProgressHelper helper)
        {
            this.doWorkDelegate = helper.doWorkDelegate;
            worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += new DoWorkEventHandler(helper.doWorkDelegate);
            CancellationTokenSourceObject = new CancellationTokenSource();

            if (helper.registedProgressChangedDeletgates != null)
            {
                RegisterHandlerProgressChanged(helper.registedProgressChangedDeletgates);
            }

            if (helper.registedWorkCompletedDelegates != null)
            {
                RegisterHandlerWorkCompleted(helper.registedWorkCompletedDelegates);
            }
        }

        // To register event handler for Progress Changed event
        public void RegisterHandlerProgressChanged(ProgressChangedDelegate progressChangedDelegate)
        {
            this.progressChangedEvent = new ProgressChangedEventHandler(progressChangedDelegate);
            worker.ProgressChanged += progressChangedEvent;
            ProgressDelegate = progressChangedDelegate;

            registedProgressChangedDeletgates = progressChangedDelegate;
        }

        // To register event handler for Run Worker completed event
        public void RegisterHandlerWorkCompleted(WorkCompletedDelegate workCompletedDelegate)
        {
            this.runWorkerCompletedEvent = new RunWorkerCompletedEventHandler(workCompletedDelegate);
            worker.RunWorkerCompleted += runWorkerCompletedEvent;

            registedWorkCompletedDelegates = workCompletedDelegate;
        }

        // To unregister event handler for Progress Changed event
        public void UnregisterHandlerProgressChanged()
        {
            worker.ProgressChanged -= progressChangedEvent;
            registedProgressChangedDeletgates = null;
        }

        // To unregister event handler for Progress Changed event
        public void UnregisterHandlerWorkCompleted()
        {
            worker.RunWorkerCompleted -= runWorkerCompletedEvent;
            registedWorkCompletedDelegates = null;
        }

        // Starting the worker
        public void StartWorker(object objectToBePassed, int workflowCount)
        {
            this.workflowCount = workflowCount;
            worker.RunWorkerAsync(objectToBePassed);
        }

        // Raise the progress event on every iteration completed by background worker
        public void ReportProgress(object result, string hostName, string hostIP)
        {
            ProgressData progressData = new ProgressData(
                result, 
                hostName, 
                hostIP, 
                this.currentWorkflowNumber,
                this.workflowCount,
                ProgressData.EventType.eventFromWorker);
            worker.ReportProgress(this.currentWorkflowNumber, progressData);
            Interlocked.Increment(ref currentWorkflowNumber);
        }

        /// <summary>
        /// Set the cancellation token
        /// </summary>
        public void SetCancellationToken()
        {                                 
            CancellationTokenSourceObject.Cancel();
        }

        public static ProgressChangedDelegate ProgressDelegate
        {
            get;
            private set;
        }    

        private int currentWorkflowNumber = 1;
        private int workflowCount = 0;

        public int WorkFlowCount
        {
            get
            {
                return this.workflowCount;
            }
        }

        public int CurrentWorkflowNumber
        {
            get
            {
                return this.currentWorkflowNumber;
            }
        }

        /// <summary>
        /// Releases all resources used by the Component.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the Component and optionally releases the managed resources
        /// </summary>
        /// <param name="disposing">set to true</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.worker != null)
                {
                    this.worker.Dispose();
                    this.worker = null;
                }
            }
        }

        /// <summary>
        /// The BackgroundWorker
        /// </summary>
        private BackgroundWorker worker;
        
        // delegates for the events
        public delegate void DoWorkDelegate(object sender, DoWorkEventArgs e);

        public delegate void ProgressChangedDelegate(object sender, ProgressChangedEventArgs e);

        public delegate void WorkCompletedDelegate(object sender, RunWorkerCompletedEventArgs e);

        /// <summary>
        /// CancellationTokenSource to contain user's choice to cancel or not
        /// Will be accessed by ParallelLoop as well as Workflow
        /// </summary>
        public static CancellationTokenSource CancellationTokenSourceObject { get; private set; }
    }
}
