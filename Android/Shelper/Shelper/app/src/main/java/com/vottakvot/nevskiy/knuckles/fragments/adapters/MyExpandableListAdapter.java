package com.vottakvot.nevskiy.knuckles.fragments.adapters;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseExpandableListAdapter;
import android.widget.TextView;

import com.vottakvot.nevskiy.knuckles.Controller;
import com.vottakvot.nevskiy.knuckles.Parser.Test;
import com.vottakvot.nevskiy.knuckles.Parser.Threads.WorkThread;
import com.vottakvot.nevskiy.knuckles.Parser.WSTask;
import com.vottakvot.nevskiy.shelper.R;

import java.util.ArrayList;
import java.util.List;

public class MyExpandableListAdapter extends BaseExpandableListAdapter {
    private Context context;

    private static List<String> listName = WorkThread.getNameTask();
    private static List<String> listResult = WorkThread.getResultList();

    public MyExpandableListAdapter(Context context){
        this.context = context;
    }

    @Override
    public int getGroupCount() {
        return listName.size();
    }

    @Override
    public int getChildrenCount(int groupPosition) {
        return 1;
    }

    @Override
    public Object getGroup(int groupPosition) {
        return null;
    }

    @Override
    public Object getChild(int groupPosition, int childPosition) {
        return null;
    }

    @Override
    public long getGroupId(int groupPosition) {
        return groupPosition;
    }

    @Override
    public long getChildId(int groupPosition, int childPosition) {
        return childPosition;
    }

    @Override
    public boolean hasStableIds() {
        return true;
    }

    @Override
    public View getGroupView(int groupPosition, boolean isExpanded, View groupView, ViewGroup parent) {
        if (groupView == null) {
            LayoutInflater inflater = (LayoutInflater) context.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
            groupView = inflater.inflate(R.layout.listview_groupview, null);
        }

        ((TextView) groupView.findViewById(R.id.question)).setText(listName.get(groupPosition));

        return groupView;
    }

    @Override
    public View getChildView(int groupPosition, int childPosition, boolean isLastChild, View childView, ViewGroup parent) {
        LayoutInflater inflater = (LayoutInflater) context.getSystemService(Context.LAYOUT_INFLATER_SERVICE);

        childView = inflater.inflate(R.layout.listview_groupview_childview, null);

        ((TextView) childView.findViewById(R.id.answer)).setText(listResult.get(groupPosition));

        return childView;
    }

    @Override
    public boolean isChildSelectable(int groupPosition, int childPosition) {
        return true;
    }
}
