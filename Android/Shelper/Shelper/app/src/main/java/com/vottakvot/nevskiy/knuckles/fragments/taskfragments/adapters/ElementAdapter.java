package com.vottakvot.nevskiy.knuckles.fragments.taskfragments.adapters;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.TextView;

import com.vottakvot.nevskiy.knuckles.MainActivity;
import com.vottakvot.nevskiy.knuckles.datatypes.Uganda;
import com.vottakvot.nevskiy.shelper.R;

import java.util.List;

/**
 * Created by Aurelius on 09.06.2018.
 */

public class ElementAdapter extends BaseAdapter {
    private List<Uganda> data;

    public ElementAdapter(List<Uganda> data){
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
            view = inflater.inflate(R.layout.elements_view_two_element, null);
        }
        ((TextView)view.findViewById(R.id.textView8)).setText(data.get(i).deWey);
        ((TextView)view.findViewById(R.id.textView9)).setText(Integer.toString(data.get(i).deWeyLength));
        return view;
    }

}
