using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime.Serialization;

[RealtimeModel]
public partial class ScoreBoardModel
{
    [RealtimeProperty(10, true, true)]
    private string _scoreText;

}

/* ----- Begin Normal Autogenerated Code ----- */
public partial class ScoreBoardModel : IModel {
    // Properties
    public string scoreText {
        get { return _cache.LookForValueInCache(_scoreText, entry => entry.scoreTextSet, entry => entry.scoreText); }
        set { if (value == scoreText) return; _cache.UpdateLocalCache(entry => { entry.scoreTextSet = true; entry.scoreText = value; return entry; }); FireScoreTextDidChange(value); }
    }
    
    // Events
    public delegate void ScoreTextDidChange(ScoreBoardModel model, string value);
    public event         ScoreTextDidChange scoreTextDidChange;
    
    // Delta updates
    private struct LocalCacheEntry {
        public bool   scoreTextSet;
        public string scoreText;
    }
    
    private LocalChangeCache<LocalCacheEntry> _cache;
    
    public ScoreBoardModel() {
        _cache = new LocalChangeCache<LocalCacheEntry>();
    }
    
    // Events
    public void FireScoreTextDidChange(string value) {
        try {
            if (scoreTextDidChange != null)
                scoreTextDidChange(this, value);
        } catch (System.Exception exception) {
            Debug.LogException(exception);
        }
    }
    
    // Serialization
    enum PropertyID {
        ScoreText = 10,
    }
    
    public int WriteLength(StreamContext context) {
        int length = 0;
        
        if (context.fullModel) {
            // Mark unreliable properties as clean and flatten the in-flight cache.
            // TODO: Move this out of WriteLength() once we have a prepareToWrite method.
            _scoreText = scoreText;
            _cache.Clear();
            
            // Write all properties
            length += WriteStream.WriteStringLength((uint)PropertyID.ScoreText, _scoreText);
        } else {
            // Reliable properties
            if (context.reliableChannel) {
                LocalCacheEntry entry = _cache.localCache;
                if (entry.scoreTextSet)
                    length += WriteStream.WriteStringLength((uint)PropertyID.ScoreText, entry.scoreText);
            }
        }
        
        return length;
    }
    
    public void Write(WriteStream stream, StreamContext context) {
        if (context.fullModel) {
            // Write all properties
            stream.WriteString((uint)PropertyID.ScoreText, _scoreText);
        } else {
            // Reliable properties
            if (context.reliableChannel) {
                LocalCacheEntry entry = _cache.localCache;
                if (entry.scoreTextSet)
                    _cache.PushLocalCacheToInflight(context.updateID);
                
                if (entry.scoreTextSet)
                    stream.WriteString((uint)PropertyID.ScoreText, entry.scoreText);
            }
        }
    }
    
    public void Read(ReadStream stream, StreamContext context) {
        bool scoreTextExistsInChangeCache = _cache.ValueExistsInCache(entry => entry.scoreTextSet);
        
        // Remove from in-flight
        if (context.deltaUpdatesOnly && context.reliableChannel)
            _cache.RemoveUpdateFromInflight(context.updateID);
        
        // Loop through each property and deserialize
        uint propertyID;
        while (stream.ReadNextPropertyID(out propertyID)) {
            switch (propertyID) {
                case (uint)PropertyID.ScoreText: {
                    string previousValue = _scoreText;
                    
                    _scoreText = stream.ReadString();
                    
                    if (!scoreTextExistsInChangeCache && _scoreText != previousValue)
                        FireScoreTextDidChange(_scoreText);
                    break;
                }
                default:
                    stream.SkipProperty();
                    break;
            }
        }
    }
}
/* ----- End Normal Autogenerated Code ----- */
