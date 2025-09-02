package hu.bme.aut.szoftlab.booking.dto;

public class TicketData {

  private String from;
  private String to;
  private String user;
  private boolean useBonus;

  public String getFrom() {
    return from;
  }

  public void setFrom(String from) {
    this.from = from;
  }

  public String getTo() {
    return to;
  }

  public void setTo(String to) {
    this.to = to;
  }

  public String getUser() {
    return user;
  }

  public void setUser(String user) {
    this.user = user;
  }

  public boolean isUseBonus() {
    return useBonus;
  }

  public void setUseBonus(boolean useBonus) {
    this.useBonus = useBonus;
  }
}
