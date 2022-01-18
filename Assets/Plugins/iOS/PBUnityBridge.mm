#import <Foundation/Foundation.h>

@interface PBUnityBridge : NSObject

@end


extern "C"
{
    void _onUnityInit()
    {
        _onUnityInit;
    }

    void _onSaving(const int *progress)
    {
		if (progress != nil) 
		{
			int prog = (int)(size_t)progress;
			[NSNotificationCenter.defaultCenter postNotificationName:@"PB_UNITY_onSaving" object:[NSNumber numberWithInt:prog]];
		}
        onSaving: progress;
    }

    void _onPlaying(const int *progress)
    {
		if (progress != nil) 
		{
            int prog = (int)(size_t)progress;
            [NSNotificationCenter.defaultCenter postNotificationName:@"PB_UNITY_onPlaying" object:[NSNumber numberWithInt:prog]];
        }
        onPlaying: progress;
    }

    void _onUnityReady(const int *totalFrames)
    {
		if (totalFrames != nil) 
		{
            int prog = (int)(size_t)totalFrames;
            [NSNotificationCenter.defaultCenter postNotificationName:@"PB_UNITY_onUnityReadyTotalFrames" object:[NSNumber numberWithInt:prog]];
        }
        onPlaying: totalFrames;
    }

    void _onStartVideo()
    {
		[NSNotificationCenter.defaultCenter postNotificationName:@"PB_UNITY_onStartVideo" object:nil];
        _onStartVideo;
    }

    void _onPauseVideo()
    {
		[NSNotificationCenter.defaultCenter postNotificationName:@"PB_UNITY_onPauseVideo" object:nil];
        _onPauseVideo;
    }

    void _onSetFrameReady()
    {
		[NSNotificationCenter.defaultCenter postNotificationName:@"PB_UNITY_onSetFrameReady" object:nil];
        _onSetFrameReady;
    }

    void _onPlayCompleted()
    {
		[NSNotificationCenter.defaultCenter postNotificationName:@"PB_UNITY_onPlayCompleted" object:nil];
        _onPlayCompleted;
    }

    void _onSavedCompleted(const char *path)
    {
		[NSNotificationCenter.defaultCenter postNotificationName:@"PB_UNITY_onSaveCompleted" object:[NSString stringWithUTF8String:path]];
        onSavedCompleted: path;
    }

    void _onFrameReady(const char *imageContents)
    {
		[NSNotificationCenter.defaultCenter postNotificationName:@"PB_UNITY_onFrameReady" object:[NSString stringWithUTF8String:imageContents]];
        onFrameReady: imageContents;
    }

    void _onImageReplaced()
    {
		[NSNotificationCenter.defaultCenter postNotificationName:@"PB_UNITY_onImageReplaced" object:nil];
        _onImageReplaced;
    }

    void _onSendMessage(const char *message)
    {
		[NSNotificationCenter.defaultCenter postNotificationName:@"PB_UNITY_onSendMessage" object:[NSString stringWithUTF8String:message]];
        onSendMessage: message;
    }

    void _onVideoSceneLoaded()
    {
		[NSNotificationCenter.defaultCenter postNotificationName:@"PB_UNITY_onVideoSceneLoaded" object:nil];
        _onVideoSceneLoaded;
    }

    void _onVideoSceneUnloaded()
    {
		[NSNotificationCenter.defaultCenter postNotificationName:@"PB_UNITY_onVideoSceneUnloaded" object:nil];
        _onVideoSceneUnloaded;
    }

    void _onUnityLoaded()
    {
		[NSNotificationCenter.defaultCenter postNotificationName:@"PB_UNITY_onUnityLoaded" object:nil];
        _onUnityLoaded;
    }
}
