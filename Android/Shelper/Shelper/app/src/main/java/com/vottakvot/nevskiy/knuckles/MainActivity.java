package com.vottakvot.nevskiy.knuckles;

import android.content.SharedPreferences;
import android.net.Uri;
import android.os.Bundle;
import android.support.annotation.NonNull;
import android.support.design.widget.BottomNavigationView;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentManager;
import android.support.v7.app.AppCompatActivity;
import android.view.MenuItem;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;
import android.widget.Toast;

import com.vottakvot.nevskiy.knuckles.Parser.Test;
import com.vottakvot.nevskiy.knuckles.Parser.Threads.ThreadSynchronize;
import com.vottakvot.nevskiy.knuckles.Parser.Threads.WorkThread;
import com.vottakvot.nevskiy.knuckles.Parser.db.DBHelper;
import com.vottakvot.nevskiy.knuckles.fragments.taskfragments.TaskCreatorFragmentFour;
import com.vottakvot.nevskiy.knuckles.fragments.taskfragments.TaskCreatorFragmentOne;
import com.vottakvot.nevskiy.knuckles.fragments.taskfragments.TaskCreatorFragmentThree;
import com.vottakvot.nevskiy.knuckles.fragments.taskfragments.TaskCreatorFragmentTwo;
import com.vottakvot.nevskiy.knuckles.fragments.taskfragments.TaskCreatorMain;
import com.vottakvot.nevskiy.knuckles.fragments.taskfragments.SettingsOfElementsOfStepThree;
import com.vottakvot.nevskiy.knuckles.fragments.ListFragment;
import com.vottakvot.nevskiy.knuckles.fragments.SettingsFragment;
import com.vottakvot.nevskiy.shelper.R;


public class MainActivity extends AppCompatActivity
        implements ListFragment.OnFragmentInteractionListener,
        SettingsFragment.OnFragmentInteractionListener,
        //TODO: rename those fucking fragments
        TaskCreatorMain.OnFragmentInteractionListener,
        TaskCreatorFragmentOne.OnFragmentInteractionListener,
        TaskCreatorFragmentTwo.OnFragmentInteractionListener,
        TaskCreatorFragmentThree.OnFragmentInteractionListener,
        TaskCreatorFragmentFour.OnFragmentInteractionListener,
        SettingsOfElementsOfStepThree.OnFragmentInteractionListener {
    private static MainActivity activity;
    private TextView mTextMessage;
    Fragment fragment = null;
    Class fragmentClass;
    FragmentManager fragmentManager = getSupportFragmentManager();

    boolean isSyn = false;

    DBHelper dbHelper;

    SharedPreferences sPref;

    private BottomNavigationView.OnNavigationItemSelectedListener mOnNavigationItemSelectedListener
            = new BottomNavigationView.OnNavigationItemSelectedListener() {

        @Override
        public boolean onNavigationItemSelected(@NonNull MenuItem item) {
            switch (item.getItemId()) {
                case R.id.navigation_home:
                    fragmentClass = ListFragment.class;
                    break;
                case R.id.navigation_dashboard:
                    //TODO: обновлять здесь
                    if (isSyn) {
                        new WorkThread(dbHelper).start();
                        isSyn = false;
                    }
                    Toast.makeText(activity, "Updating", Toast.LENGTH_SHORT).show();
                    return false;
                case R.id.navigation_notifications:
                    fragmentClass = SettingsFragment.class;
                    break;
                default:
                    fragmentClass = ListFragment.class;
            }

            try {
                fragment = (Fragment) fragmentClass.newInstance();
            } catch (Exception e) {
                e.printStackTrace();
            }

            fragmentManager.beginTransaction().replace(R.id.list_item, fragment).commit();
            return true;
        }
    };

    public void saveText(String tag, String text) {
        sPref = getPreferences(MODE_PRIVATE);
        SharedPreferences.Editor ed = sPref.edit();
        ed.putString(tag, text);
        ed.commit();
    }

    public String loadText(String tag) {
        sPref = getPreferences(MODE_PRIVATE);
        String savedText = sPref.getString(tag, "");
        return savedText;
    }

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        activity = this;

        mTextMessage = (TextView) findViewById(R.id.message);
        BottomNavigationView navigation = (BottomNavigationView) findViewById(R.id.navigation);
        navigation.setOnNavigationItemSelectedListener(mOnNavigationItemSelectedListener);

        // создаем объект для создания и управления версиями БД
        this.deleteDatabase("wstask.db");
        dbHelper = new DBHelper(this, false);

        //new Test(dbHelper).start();
        //new ThreadSynchronize(dbHelper.getmDataBase()).start();
        new WorkThread(dbHelper).start();
    }

    public static MainActivity getInstance() {
        return activity;
    }

    //TODO: rename

    /**
     * begins a wonderful journey in the world of anuses
     */
    public void openTaskDialog() throws IllegalAccessException, InstantiationException {
        fragmentClass = TaskCreatorMain.class;
        fragment = (Fragment) fragmentClass.newInstance();
        fragmentManager.beginTransaction().replace(R.id.list_item, fragment).commit();
    }

    @Override
    public void onFragmentInteraction(Uri uri) {

    }

    public void setSyn(boolean syn) {
        isSyn = syn;
    }
}
