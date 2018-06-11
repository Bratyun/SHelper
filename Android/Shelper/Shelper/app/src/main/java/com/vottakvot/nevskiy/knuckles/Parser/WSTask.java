package com.vottakvot.nevskiy.knuckles.Parser;

import android.util.Log;

import com.vottakvot.nevskiy.knuckles.Parser.data.ContainerPath;
import com.vottakvot.nevskiy.knuckles.Parser.data.Data;
import com.vottakvot.nevskiy.knuckles.Parser.data.DataTag;
import com.vottakvot.nevskiy.knuckles.Parser.data.Element;
import com.vottakvot.nevskiy.knuckles.Parser.data.ElementPath;
import com.vottakvot.nevskiy.knuckles.Parser.data.Result;

import org.jsoup.Jsoup;
import org.jsoup.nodes.Document;
import org.jsoup.select.Elements;

import java.io.IOException;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

/**
 * Created by Shuyning on 03.06.2018.
 */

public class WSTask {
    private int id;
    private String name;
    private String link;

    private Elements container;

    private ContainerPath containerStepTwo;
    private ElementPath elementContainer;

    private List<Result> resultList = new ArrayList<>();

    ////////////////////////////////////////////////////
    // Constructors
    ///////////////////////////////////////////////////


    public WSTask(String name, String link) {
        this.name = name;
        this.link = link;

        elementContainer = new ElementPath();
        elementContainer.setName(name);

        setBaseContainer();
    }

    ////////////////////////////////////////////////////
    // Getters amd Setters
    ///////////////////////////////////////////////////


    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public ContainerPath getContainerStepTwo() {
        if (containerStepTwo == null){
            containerStepTwo = new ContainerPath();
        }
        return containerStepTwo;
    }

    public void setContainerStepTwo(ContainerPath containerStepTwo) {
        this.containerStepTwo = containerStepTwo;
    }

    public List<Result> getResultResp() {
        return resultList;
    }

    public void setResultResp(Result resultResp) {
        if (resultResp != null){
            resultList.add(resultResp);
        }
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public String getLink() {
        return link;
    }

    public void setLink(String link) {
        this.link = link;
    }

    public void setContainer(Elements container) {
        this.container = container;
    }

    public List<Result> getResultList() {
        return resultList;
    }

    public void setResultList(List<Result> resultList) {
        this.resultList = resultList;
    }

    ////////////////////////////////////////////////////
    // Base methods
    ///////////////////////////////////////////////////

    private void setBaseContainer() {
        try {
            Document document = Jsoup.connect(link).get();
            container = document.select("body");
        } catch (IOException e) {
            Log.e("Connection", "Не удалось получить доступ к сайту!", e);
        }
    }

    public void startStepTwo(String tagName, String where, String distance) {
        containerStepTwo = new ContainerPath();

        containerStepTwo.setTag(tagName);
        containerStepTwo.setWhere(where);
        containerStepTwo.setDistance(distance);
    }

    public void setDataToTwoStep(String purpose, String properties, String howSearch, String value) {
        containerStepTwo.addData(purpose, properties, howSearch, value);
    }

    public void deleteTwoStep() {
        containerStepTwo = null;
    }

    public void process() {
        Elements elements;
        Elements elementsCopy = null;
        Result resultResp = new Result();

        String result;

        if (containerStepTwo != null) {
            elements = prepareContainerTwoStep();
            elementsCopy = elements;
        }
        if (elementContainer != null) {
            result = getResult(elementsCopy);
            resultResp.setResult(result);
            resultList.add(resultResp);
        }
    }

    public void printResult() {

    }

    private Elements prepareContainerTwoStep() {
        Elements elements = null;
        Elements elements1 = new Elements();
        StringBuilder pathToElement = new StringBuilder();

        String result = "";

        if (containerStepTwo.getDataList() != null) {
            for (Data data : containerStepTwo.getDataList()) {
                if (data.getProperties() != null && data.getValue() != null) {
                    if (data.getProperties().equals("class")) {
                        if (container.select(containerStepTwo.getTag() + "." + data.getValue()) != null) {
                            String request = containerStepTwo.getTag() + "[class=" + data.getValue() + "]";
                            elements = container.select(request);
                        }
                    } else {
                        if (!data.getValue().equals("null")) {
                            String request = containerStepTwo.getTag() + "[" + data.getProperties() + "=" + data.getValue() + "]";
                            elements = container.select(request);
                        }
                    }
                }
                if (data.getProperties() != null && data.getValue() == null) {
                    if (elements != null) {
                        result = elements.attr(data.getProperties());
                    } else {
                        //Обработать ошибку
                        result = container.attr(data.getProperties());
                        result = container.attr("href");
                    }
                }
                if (data.getProperties() == null && data.getValue() == null) {
                    //Обработать ошибку
                    if (elements != null) {
                        result = elements.text();
                    } else {
                        result = container.text();
                    }
                }
            }
        }

        return elements;
    }

    private String checkForLocationTag() {
        if ("in".equals(containerStepTwo.getWhere())) {
            return "in";
        }
        if ("around".equals(containerStepTwo.getWhere())) {
            return "around";
        }
        if ("below".equals(containerStepTwo.getWhere())) {
            return "below";
        }
        if ("higher".equals(containerStepTwo.getWhere())) {
            return "higher";
        }

        return "in";
    }

    ////////////////////////////////////////////////////
    // task 3 methods
    ///////////////////////////////////////////////////


    public ElementPath getElementContainer() {
        return elementContainer;
    }

    public void setElementContainer(ElementPath elementContainer) {
        this.elementContainer = elementContainer;
    }

    public void addDataTagToElementContainer(List<DataTag> dataTags) {
        List<DataTag> dataTag = elementContainer.getDataTagList();

        for (DataTag dataTag1 : dataTags) {
            dataTag.add(dataTag1);
        }

        elementContainer.setDataTagList(dataTag);
        String s;
    }

    public Elements getElementsStepThree() {
        return null;
    }

    private String getResult(Elements elementsCopy) {
        Elements elements = null;
        String result = "";
        String s = "";

        int count = 1;

        String path = "";

        if (elementsCopy != null) {
            for (DataTag dataTag : elementContainer.getDataTagList()) {
                for (Element element : dataTag.getElementList()) {
                    int i = 0;

                    path = path.concat(element.getTag());
                    elements = elementsCopy;

                    /*if (s.equals(result)) {
                        i = 1;
                    } else {
                        i = 2;
                    }

                    s = result;
                    count = i;
                    while (i != 0) {*/
                    int countData = 1;
                    for (Data data : element.getDataList()) {
                        Map<String, String> map = null;
                        map = prepareDataResponce(data);

                        if (map.containsKey("a")) {
                            path = path.concat(map.get("a"));
                            if ((elements = elementsCopy.select(path)) != null) {
                                continue;
                            }
                        }
                        if (map.containsKey("b")) {
                            Elements elem = elements.get(0).getElementsByAttribute(map.get("b"));
                            result = result.concat("\n").concat(elem.attr(map.get("b")));
                        }
                        if (map.containsKey("c")) {
                            if (count == 1){
                                Elements elem = elements.get(0).getElementsByTag(element.getTag());
                                result = result.concat("\n").concat(elem.first().text());
                                continue;
                            }
                            result = result.concat("\n").concat(elements.get(0).text());
                        }
                        count++;
                    }
/*
                        i--;
                    }*/

                   /* if (count == 2) {*/
                    elements = null;
                      /*  count = 1;
                    }*/

                    path = "";
                }
            }
        }

        return result;
    }

    private Map<String, String> prepareDataResponce(Data data) {
        Map<String, String> map = new HashMap<>();
        String result = "";

        if (!data.getProperties().equals("") && !data.getValue().equals("") || !data.getProperties().equals("") && !data.getValue().equals("null")) {
            result = result.concat("[").concat(data.getProperties()).concat("=").concat(data.getValue()).concat("]");
            map.put("a", result);
            return map;
        }
        if (!data.getProperties().equals("") && data.getValue().equals("") || !data.getProperties().equals("") && data.getValue().equals("null")) {
            result = result.concat(data.getProperties());
            map.put("b", result);
            return map;
        }
        if (data.getProperties().equals("") && data.getValue().equals("") || data.getProperties().equals("") && data.getValue().equals("null")) {
            map.put("c", result);
            return map;
        }

        return map;
    }
}
