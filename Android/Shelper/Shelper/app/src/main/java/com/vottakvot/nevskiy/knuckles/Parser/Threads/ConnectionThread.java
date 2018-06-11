package com.vottakvot.nevskiy.knuckles.Parser.Threads;

import com.vottakvot.nevskiy.knuckles.fragments.taskfragments.TaskCreatorFragmentOne;

import org.jsoup.Connection;
import org.jsoup.Jsoup;
import org.jsoup.nodes.Document;

import java.io.IOException;

/**
 * Created by Shuyning on 10.06.2018.
 */

public class ConnectionThread extends Thread {
    static StringBuilder builder;
    private static boolean result = false;
    Document document;

    public static void getConnection(String urlCon) {
        builder = new StringBuilder();
        builder.append("https://");
        if (urlCon.length() != 0) {
            builder.append(urlCon);
        }

        ConnectionThread connectionThread = new ConnectionThread();
        connectionThread.start();
        try {
            Thread.sleep(5000);
        } catch (InterruptedException e) {
            e.printStackTrace();
        }

        TaskCreatorFragmentOne.getInstance().setResTestCon(result);
    }

    public boolean isResult() {
        return result;
    }

    @Override
    public void run() {
        try {
            if (Jsoup.connect(builder.toString()).get() != null) {
                result = true;
            } else {
                result = false;
            }
        } catch (IOException e) {
            result = false;
        }
    }
}
