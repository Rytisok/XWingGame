using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime.Serialization;

[RealtimeModel]
public partial class TrailModel
{
    [RealtimeProperty(3, true, true)]
    private int _health;
}

/* ----- Begin Normal Autogenerated Code ----- */
public partial class TrailModel : IModel {
    // Properties
    public int health {
        get { return _cache.LookForValueInCache(_health, entry => entry.healthSet, entry => entry.health); }
        set { if (value == health) return; _cache.UpdateLocalCache(entry => { entry.healthSet = true; entry.health = value; return entry; }); FireHealthDidChange(value); }
    }
    
    // Events
    public delegate void HealthDidChange(TrailModel model, int value);
    public event         HealthDidChange healthDidChange;
    
    // Delta updates
    private struct LocalCacheEntry {
        public bool healthSet;
        public int  health;
    }
    
    private LocalChangeCache<LocalCacheEntry> _cache;
    
    public TrailModel() {
        _cache = new LocalChangeCache<LocalCacheEntry>();
    }
    
    // Events
    public void FireHealthDidChange(int value) {
        try {
            if (healthDidChange != null)
                healthDidChange(this, value);
        } catch (System.Exception exception) {
            Debug.LogException(exception);
        }
    }
    
    // Serialization
    enum PropertyID {
        Health = 3,
    }
    
    public int WriteLength(StreamContext context) {
        int length = 0;
        
        if (context.fullModel) {
            // Mark unreliable properties as clean and flatten the in-flight cache.
            // TODO: Move this out of WriteLength() once we have a prepareToWrite method.
            _health = health;
            _cache.Clear();
            
            // Write all properties
            length += WriteStream.WriteVarint32Length((uint)PropertyID.Health, (uint)_health);
        } else {
            // Reliable properties
            if (context.reliableChannel) {
                LocalCacheEntry entry = _cache.localCache;
                if (entry.healthSet)
                    length += WriteStream.WriteVarint32Length((uint)PropertyID.Health, (uint)entry.health);
            }
        }
        
        return length;
    }
    
    public void Write(WriteStream stream, StreamContext context) {
        if (context.fullModel) {
            // Write all properties
            stream.WriteVarint32((uint)PropertyID.Health, (uint)_health);
        } else {
            // Reliable properties
            if (context.reliableChannel) {
                LocalCacheEntry entry = _cache.localCache;
                if (entry.healthSet)
                    _cache.PushLocalCacheToInflight(context.updateID);
                
                if (entry.healthSet)
                    stream.WriteVarint32((uint)PropertyID.Health, (uint)entry.health);
            }
        }
    }
    
    public void Read(ReadStream stream, StreamContext context) {
        bool healthExistsInChangeCache = _cache.ValueExistsInCache(entry => entry.healthSet);
        
        // Remove from in-flight
        if (context.deltaUpdatesOnly && context.reliableChannel)
            _cache.RemoveUpdateFromInflight(context.updateID);
        
        // Loop through each property and deserialize
        uint propertyID;
        while (stream.ReadNextPropertyID(out propertyID)) {
            switch (propertyID) {
                case (uint)PropertyID.Health: {
                    int previousValue = _health;
                    
                    _health = (int)stream.ReadVarint32();
                    
                    if (!healthExistsInChangeCache && _health != previousValue)
                        FireHealthDidChange(_health);
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
