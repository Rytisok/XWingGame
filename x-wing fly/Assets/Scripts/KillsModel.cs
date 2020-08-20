using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime.Serialization;

[RealtimeModel]
public partial class KillsModel
{
    [RealtimeProperty(16, true, true)]
    private int _killCount;
}

/* ----- Begin Normal Autogenerated Code ----- */
public partial class KillsModel : IModel {
    // Properties
    public int killCount {
        get { return _cache.LookForValueInCache(_killCount, entry => entry.killCountSet, entry => entry.killCount); }
        set { if (value == killCount) return; _cache.UpdateLocalCache(entry => { entry.killCountSet = true; entry.killCount = value; return entry; }); FireKillCountDidChange(value); }
    }
    
    // Events
    public delegate void KillCountDidChange(KillsModel model, int value);
    public event         KillCountDidChange killCountDidChange;
    
    // Delta updates
    private struct LocalCacheEntry {
        public bool killCountSet;
        public int  killCount;
    }
    
    private LocalChangeCache<LocalCacheEntry> _cache;
    
    public KillsModel() {
        _cache = new LocalChangeCache<LocalCacheEntry>();
    }
    
    // Events
    public void FireKillCountDidChange(int value) {
        try {
            if (killCountDidChange != null)
                killCountDidChange(this, value);
        } catch (System.Exception exception) {
            Debug.LogException(exception);
        }
    }
    
    // Serialization
    enum PropertyID {
        KillCount = 16,
    }
    
    public int WriteLength(StreamContext context) {
        int length = 0;
        
        if (context.fullModel) {
            // Mark unreliable properties as clean and flatten the in-flight cache.
            // TODO: Move this out of WriteLength() once we have a prepareToWrite method.
            _killCount = killCount;
            _cache.Clear();
            
            // Write all properties
            length += WriteStream.WriteVarint32Length((uint)PropertyID.KillCount, (uint)_killCount);
        } else {
            // Reliable properties
            if (context.reliableChannel) {
                LocalCacheEntry entry = _cache.localCache;
                if (entry.killCountSet)
                    length += WriteStream.WriteVarint32Length((uint)PropertyID.KillCount, (uint)entry.killCount);
            }
        }
        
        return length;
    }
    
    public void Write(WriteStream stream, StreamContext context) {
        if (context.fullModel) {
            // Write all properties
            stream.WriteVarint32((uint)PropertyID.KillCount, (uint)_killCount);
        } else {
            // Reliable properties
            if (context.reliableChannel) {
                LocalCacheEntry entry = _cache.localCache;
                if (entry.killCountSet)
                    _cache.PushLocalCacheToInflight(context.updateID);
                
                if (entry.killCountSet)
                    stream.WriteVarint32((uint)PropertyID.KillCount, (uint)entry.killCount);
            }
        }
    }
    
    public void Read(ReadStream stream, StreamContext context) {
        bool killCountExistsInChangeCache = _cache.ValueExistsInCache(entry => entry.killCountSet);
        
        // Remove from in-flight
        if (context.deltaUpdatesOnly && context.reliableChannel)
            _cache.RemoveUpdateFromInflight(context.updateID);
        
        // Loop through each property and deserialize
        uint propertyID;
        while (stream.ReadNextPropertyID(out propertyID)) {
            switch (propertyID) {
                case (uint)PropertyID.KillCount: {
                    int previousValue = _killCount;
                    
                    _killCount = (int)stream.ReadVarint32();
                    
                    if (!killCountExistsInChangeCache && _killCount != previousValue)
                        FireKillCountDidChange(_killCount);
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
