package com.vottakvot.nevskiy.knuckles.Parser.Threads;

import com.vottakvot.nevskiy.knuckles.Parser.WSTask;
import com.vottakvot.nevskiy.knuckles.Parser.data.ContainerPath;
import com.vottakvot.nevskiy.knuckles.Parser.data.ElementPath;

import org.jsoup.select.Elements;

/**
 * Created by Shuyning on 11.06.2018.
 */

public class CreateTaskThread extends Thread {
    private WSTask wsTask;

    private static CreateTaskThread instance = null;

    private String name;
    private String link;

    private Elements container;

    private ContainerPath containerStepTwo;
    private ElementPath elementContainer;

    public static synchronized CreateTaskThread getInstance(){
        if (instance == null){
            instance = new CreateTaskThread();
        }
        return instance;
    }

    public WSTask getWsTask() {
        return wsTask;
    }

    public void setWsTask(WSTask wsTask) {
        this.wsTask = wsTask;
    }

    public String getNameTask() {
        return name;
    }

    public String getLink() {
        return link;
    }

    public Elements getContainer() {
        return container;
    }

    public ContainerPath getContainerStepTwo() {
        return containerStepTwo;
    }

    public ElementPath getElementContainer() {
        return elementContainer;
    }

    public void setNameTask(String name) {
        this.name = name;
    }

    public void setLink(String link) {
        this.link = link;
    }

    public void setContainer(Elements container) {
        this.container = container;
    }

    public void setContainerStepTwo(ContainerPath containerStepTwo) {
        this.containerStepTwo = containerStepTwo;
    }

    public void setElementContainer(ElementPath elementContainer) {
        this.elementContainer = elementContainer;
    }

    @Override
    public void run() {
        super.run();
    }
}
