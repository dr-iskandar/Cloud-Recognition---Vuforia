﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

namespace UnityLibrary {
    public class SimpleCloudHandler : MonoBehaviour, IObjectRecoEventHandler
    {
        private CloudRecoBehaviour mCloudRecoBehaviour;
        private bool mIsScanning = false;
        private string mTargetMetadata = "";

        public GameObject restartButton;
        public LoadAssets loadAssets;

        void Start()
        {
            // register this event handler at the cloud reco behaviour 
            mCloudRecoBehaviour = GetComponent<CloudRecoBehaviour>();

            if (mCloudRecoBehaviour)
            {
                mCloudRecoBehaviour.RegisterEventHandler(this);
            }
        }

        public void OnInitialized(TargetFinder targetFinder)
        {
            Debug.Log("Cloud Reco initialized");
        }
        public void OnInitError(TargetFinder.InitState initError)
        {
            Debug.Log("Cloud Reco init error " + initError.ToString());
        }
        public void OnUpdateError(TargetFinder.UpdateState updateError)
        {
            Debug.Log("Cloud Reco update error " + updateError.ToString());
        }

        public void OnStateChanged(bool scanning)
        {
            mIsScanning = scanning;
            if (scanning)
            {
                // clear all known trackables
                var tracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
                tracker.GetTargetFinder<ImageTargetFinder>().ClearTrackables(false);
                restartButton.SetActive(false);
            }
        }

        // Here we handle a cloud target recognition event
        public void OnNewSearchResult(TargetFinder.TargetSearchResult targetSearchResult)
        {
            TargetFinder.CloudRecoSearchResult cloudRecoSearchResult = (TargetFinder.CloudRecoSearchResult)targetSearchResult;

            // do something with the target metadata
            mTargetMetadata = cloudRecoSearchResult.MetaData;
            loadAssets.contentParent = transform;
            loadAssets.namaaset = mTargetMetadata;
            loadAssets.LoadContent();
            restartButton.SetActive(true);

            // stop the target finder (i.e. stop scanning the cloud)
            mCloudRecoBehaviour.CloudRecoEnabled = false;
        }

        public void RestartScanning()
        {
            mCloudRecoBehaviour.CloudRecoEnabled = true;
        }

        //void OnGUI()
        //{
        //    // Display current 'scanning' status
        //    GUI.Box(new Rect(100, 100, 200, 50), mIsScanning ? "Scanning" : "Not scanning");
        //    // Display metadata of latest detected cloud-target
        //    GUI.Box(new Rect(100, 200, 200, 50), "Metadata: " + mTargetMetadata);
        //    // If not scanning, show button
        //    // so that user can restart cloud scanning
        //    if (!mIsScanning)
        //    {
        //        if (GUI.Button(new Rect(100, 300, 200, 50), "Restart Scanning"))
        //        {
        //            // Restart TargetFinder
        //           
        //        }
        //    }
        //}
    }
}