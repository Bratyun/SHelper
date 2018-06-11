package com.vottakvot.nevskiy.knuckles.fragments.taskfragments.adapters;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.EditText;
import android.widget.TextView;

import com.vottakvot.nevskiy.knuckles.MainActivity;
import com.vottakvot.nevskiy.knuckles.datatypes.DeWey;
import com.vottakvot.nevskiy.shelper.R;

import java.util.List;

/**
 * Created by Aurelius on 09.06.2018.
 */

public class ElementSettingsAdapter extends BaseAdapter {
    private List<DeWey> data;

    public ElementSettingsAdapter(List<DeWey> data){
        this.data=data;
    }
    @Override
    public int getCount() {
        return data.size();
    }

    @Override
    public Object getItem(int i) {
        return data.get(i);
    }

    @Override
    public long getItemId(int i) {
        return i;
    }

    @Override
    public View getView(int i, View view, ViewGroup viewGroup) {
        if (view == null) {
            LayoutInflater inflater = (LayoutInflater) MainActivity.getInstance().getSystemService(Context.LAYOUT_INFLATER_SERVICE);
            view = inflater.inflate(R.layout.element_settings_view_one_element, null);
        }
        ((TextView)view.findViewById(R.id.indexText)).setText(Integer.toString(i+1));
        ((EditText)view.findViewById(R.id.editText1)).setText(data.get(i).auto);
        ((EditText)view.findViewById(R.id.editText2)).setText(data.get(i).purpose);
        return view;
    }

}
