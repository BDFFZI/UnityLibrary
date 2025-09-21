package com.company.product;
import com.unity3d.player.UnityPlayerActivity;
import android.os.Bundle;
import android.util.Log;

import java.util.function.Supplier;
import android.view.KeyEvent;
import android.widget.Toast;

public class ActivityProxyInterface extends UnityPlayerActivity {
    public Supplier<Boolean> VolumeDownHandler;
    public Supplier<Boolean> VolumeUpHandler;

    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
    }

    @Override
    public boolean onKeyDown(int keyCode, KeyEvent event) {
        if (VolumeDownHandler != null && keyCode == KeyEvent.KEYCODE_VOLUME_DOWN) {
            return VolumeDownHandler.get();
        }
        if (VolumeUpHandler != null && keyCode == KeyEvent.KEYCODE_VOLUME_UP) {
            return VolumeUpHandler.get();
        }

        return super.onKeyDown(keyCode, event);
    }
}