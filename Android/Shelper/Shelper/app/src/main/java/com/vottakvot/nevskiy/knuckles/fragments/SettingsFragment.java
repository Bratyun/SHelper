package com.vottakvot.nevskiy.knuckles.fragments;

import android.content.Context;
import android.net.Uri;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.support.v7.widget.AppCompatEditText;
import android.view.KeyEvent;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.CompoundButton;
import android.widget.Switch;
import android.widget.Toast;

import com.vottakvot.nevskiy.knuckles.MainActivity;
import com.vottakvot.nevskiy.knuckles.Parser.Threads.ThreadSynchronize;
import com.vottakvot.nevskiy.knuckles.Parser.db.DBHelper;
import com.vottakvot.nevskiy.shelper.R;


/**
 * A simple {@link Fragment} subclass.
 * Activities that contain this fragment must implement the
 * {@link SettingsFragment.OnFragmentInteractionListener} interface
 * to handle interaction events.
 * Use the {@link SettingsFragment#newInstance} factory method to
 * create an instance of this fragment.
 */
public class SettingsFragment extends Fragment {
    private OnFragmentInteractionListener mListener;

    private AppCompatEditText repetitionPause;
    private AppCompatEditText changePause;
    private Switch beginOnLaunch;
    private Switch workConstantly;
    private Button makeDefault;
    private Button setNewSite;
    private Button buttonSynch;
    public static final String REP_PAUSE = "repetitionPause";
    public static final String CH_PAUSE = "changePause";
    public static final String BEGIN_ON_LAUNCH = "beginOnLaunch";
    public static final String WORK_CONST = "workConstantly";
    MainActivity mainActivity;

    public SettingsFragment() {
        // Required empty public constructor
    }

    public static SettingsFragment newInstance() {
        SettingsFragment fragment = new SettingsFragment();
        Bundle args = new Bundle();
        fragment.setArguments(args);
        return fragment;
    }

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        mainActivity = (MainActivity)this.getActivity();
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        View v = inflater.inflate(R.layout.fragment_settings, container, false);

        repetitionPause = (AppCompatEditText)v.findViewById(R.id.editText);
        repetitionPause.setText(mainActivity.loadText(REP_PAUSE));
        repetitionPause.setOnKeyListener(new View.OnKeyListener() {
            public boolean onKey(View v, int keyCode, KeyEvent event) {
                if ((event.getAction() == KeyEvent.ACTION_DOWN) && (keyCode == KeyEvent.KEYCODE_ENTER)) {
                    mainActivity.saveText(REP_PAUSE,repetitionPause.getText().toString());
                    return true;
                }
                return false;
            }
        });
        changePause = (AppCompatEditText)v.findViewById(R.id.editText2);
        changePause.setText(mainActivity.loadText(CH_PAUSE));
        changePause.setOnKeyListener(new View.OnKeyListener() {
            public boolean onKey(View v, int keyCode, KeyEvent event) {
                if ((event.getAction() == KeyEvent.ACTION_DOWN) && (keyCode == KeyEvent.KEYCODE_ENTER)) {
                    mainActivity.saveText(CH_PAUSE,changePause.getText().toString());
                    return true;
                }
                return false;
            }
        });
        beginOnLaunch = (Switch)v.findViewById(R.id.switch1);
        beginOnLaunch.setChecked(Boolean.parseBoolean(mainActivity.loadText(BEGIN_ON_LAUNCH)));
        beginOnLaunch.setOnCheckedChangeListener(new Switch.OnCheckedChangeListener(){

            @Override
            public void onCheckedChanged(CompoundButton compoundButton, boolean b) {
                mainActivity.saveText(BEGIN_ON_LAUNCH,Boolean.toString(b));
            }
        });
        workConstantly = (Switch)v.findViewById(R.id.switch2);
        workConstantly.setChecked(Boolean.parseBoolean(mainActivity.loadText(WORK_CONST)));
        workConstantly.setOnCheckedChangeListener(new Switch.OnCheckedChangeListener(){

            @Override
            public void onCheckedChanged(CompoundButton compoundButton, boolean b) {
                mainActivity.saveText(WORK_CONST,Boolean.toString(b));
            }
        });
        makeDefault = (Button)v.findViewById(R.id.button);
        makeDefault.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Toast.makeText(mainActivity, "Click-Click", Toast.LENGTH_SHORT).show();
            }
        });

        setNewSite = (Button)v.findViewById(R.id.button2);
        setNewSite.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                try {
                    mainActivity.openTaskDialog();
                } catch (IllegalAccessException e) {
                    e.printStackTrace();
                } catch (java.lang.InstantiationException e) {
                    e.printStackTrace();
                }
            }
        });

        buttonSynch = (Button)v.findViewById(R.id.buttonSynch);
        buttonSynch.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                DBHelper dbHelper = new DBHelper(MainActivity.getInstance(), true);
                new ThreadSynchronize(dbHelper.getmDataBase()).start();
                Toast.makeText(MainActivity.getInstance(), "Synchronization", Toast.LENGTH_SHORT).show();
            }
        });

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
