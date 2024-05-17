package com.codingpilot.mauigoogleplayservices;

import android.app.Activity;
import android.app.Application;
import android.content.Context;
import android.content.IntentSender;
import android.os.Bundle;
import android.util.Log;

import androidx.activity.result.ActivityResultLauncher;
import androidx.activity.result.IntentSenderRequest;

import com.google.android.gms.tasks.Task;
import com.google.android.play.core.appupdate.AppUpdateInfo;
import com.google.android.play.core.appupdate.AppUpdateManager;
import com.google.android.play.core.appupdate.AppUpdateManagerFactory;
import com.google.android.play.core.appupdate.AppUpdateOptions;
import com.google.android.play.core.install.model.AppUpdateType;
import com.google.android.play.core.install.model.UpdateAvailability;

import kotlinx.coroutines.flow.internal.SendingCollector;


public class AppUpdateManagerSdk {

    public interface IFailureListener {
        void OnFailure(Exception exception);
    }

    public interface ISuccessListener {
        void OnSuccess(AppUpdateInfo appUpdateInfo);
    }

    public void CheckForUpdate(Context context, ISuccessListener successListener, IFailureListener failureListener)  {
        AppUpdateManager appUpdateManager = AppUpdateManagerFactory.create(context);

// Returns an intent object that you use to check for an update.
        Task<AppUpdateInfo> appUpdateInfoTask = appUpdateManager.getAppUpdateInfo();


// Checks that the platform will allow the specified type of update.
        appUpdateInfoTask
                .addOnFailureListener(failure -> { failureListener.OnFailure(failure); } )
                .addOnSuccessListener(appUpdateInfo -> { successListener.OnSuccess(appUpdateInfo); });
    }

//    public static boolean InstallFlexible(AppUpdateInfo appUpdateInfo, Activity activity) throws IntentSender.SendIntentException {
//        return appUpdateManager.startUpdateFlowForResult(
//                appUpdateInfo,
//                activity,
//                AppUpdateOptions.newBuilder(AppUpdateType.FLEXIBLE).build(),
//                1);
//
//    }



}