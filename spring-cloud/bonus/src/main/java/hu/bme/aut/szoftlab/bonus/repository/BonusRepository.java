package hu.bme.aut.szoftlab.bonus.repository;

import hu.bme.aut.szoftlab.bonus.model.Bonus;
import org.springframework.data.jpa.repository.JpaRepository;

public interface BonusRepository extends JpaRepository<Bonus, String>{

}
