package com.vottakvot.nevskiy.knuckles.fragments.taskfragments;

import android.content.Context;
import android.net.Uri;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.ListAdapter;
import android.widget.ListView;

import com.vottakvot.nevskiy.knuckles.fragments.taskfragments.adapters.ElementSettingsAdapter;
import com.vottakvot.nevskiy.knuckles.Controller;
import com.vottakvot.nevskiy.knuckles.datatypes.DeWey;
import com.vottakvot.nevskiy.shelper.R;

/**
 * A simple {@link Fragment} subclass.
 * Activities that contain this fragment must implement the
 * {@link TaskCreatorFragmentTwo.OnFragmentInteractionListener} interface
 * to handle interaction events.
 * Use the {@link TaskCreatorFragmentTwo#newInstance} factory method to
 * create an instance of this fragment.
 */
public class TaskCreatorFragmentTwo extends Fragment {
    // TODO: Rename parameter arguments, choose names that match
    // the fragment initialization parameters, e.g. ARG_ITEM_NUMBER
    ListView listView;
    Button addButton;
    Button deleteButton;
    ElementSettingsAdapter elementSettingsAdapter;

    // TODO: Rename and change types of parameters

    private OnFragmentInteractionListener mListener;

    public TaskCreatorFragmentTwo() {
        // Required empty public constructor
    }

    /**
     * Use this factory method to create a new instance of
     * this fragment using the provided parameters.
     * @return A new instance of fragment TaskCreatorFragmentTwo.
     */
    // TODO: Rename and change types and number of parameters
    public static TaskCreatorFragmentTwo newInstance() {
        TaskCreatorFragmentTwo fragment = new TaskCreatorFragmentTwo();
        Bundle args = new Bundle();
        fragment.setArguments(args);
        return fragment;
    }

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        View v = inflater.inflate(R.layout.task_creator_fragment_two, container, false);

        elementSettingsAdapter = new ElementSettingsAdapter(Controller.deWeys);

        listView = (ListView)v.findViewById(R.id.analGListView1);
        listView.setAdapter(elementSettingsAdapter);

        addButton = (Button)v.findViewById(R.id.addButton);
        addButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Controller.deWeys.add(new DeWey());
                elementSettingsAdapter.notifyDataSetChanged();
            }
        });
        deleteButton = (Button)v.findViewById(R.id.deleteButton);
        deleteButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                if(Controller.deWeys.size()>0)
                Controller.deWeys.remove(Controller.deWeys.size()-1);
                elementSettingsAdapter.notifyDataSetChanged();
            }
        });
        // Inflate the layout for this fragment
        return v;
    }

    // TODO: Rename method, update argument and hook method into UI event
    public void onButtonPressed(Uri uri) {
        if (mListener != null) {
            mListener.onFragmentInteraction(uri);
        }
    }

    @Override
    public void onAttach(Context context) {
        super.onAttach(context);
        if (context instanceof OnFragmentInteractionListener) {
            mListener = (OnFragmentInteractionListener) context;
        } else {
            throw new RuntimeException(context.toString()
                    + " must implement OnFragmentInteractionListener");
        }
    }

    @Override
    public void onDetach() {
        super.onDetach();
        mListener = null;
    }

    /**
     * This interface must be implemented by activities that contain this
     * fragment to allow an interaction in this fragment to be communicated
     * to the activity and potentially other fragments contained in that
     * activity.
     * <p>
     * See the Android Training lesson <a href=
     * "http://developer.android.com/training/basics/fragments/communicating.html"
     * >Communicating with Other Fragments</a> for more information.
     */
    public interface OnFragmentInteractionListener {
        // TODO: Update argument type and name
        void onFragmentInteraction(Uri uri);
    }
}
