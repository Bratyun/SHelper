package com.vottakvot.nevskiy.knuckles.Parser.data;

import java.util.ArrayList;
import java.util.List;

/**
 * Created by Shuyning on 06.06.2018.
 */

public class DataTag {
    private int id;

    private int quantity;
    private int numbeReturneData;
    private String start;

    private List<Element> elementList = new ArrayList<>();

    public void setElementList(List<Element> elementList) {
        this.elementList = elementList;
    }

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public int getQuantity() {
        return quantity;
    }

    public List<Element> getElementList(){
        return elementList;
    }

    public void setQuantity(int quantity) {
        this.quantity = quantity;
    }

    public int getNumbeReturneData() {
        return numbeReturneData;
    }

    public void setNumbeReturneData(int numbeReturneData) {
        this.numbeReturneData = numbeReturneData;
    }

    public String getStart() {
        return start;
    }

    public void setStart(String start) {
        this.start = start;
    }

    public void addElement(String tag, String eLocation, String eDistance, Data... listData){
        Element element = new Element();

        element.setTag(tag);
        element.seteLocation(eLocation);
        element.seteDistance(eDistance);

        for (Data data: listData) {
            element.addDataList(data);
        }
    }

    public void addDataList(int quantity, int numbeReturneData, String start, List<Element> elements){

        this.setQuantity(quantity);
        this.setNumbeReturneData(numbeReturneData);
        this.setStart(start);

        for (Element element: elements){
            elementList.add(element);
        }
    }
}
