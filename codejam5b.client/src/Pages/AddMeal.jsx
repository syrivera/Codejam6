import { useState } from 'react';
import './AddMeal.css';

function AddMeal() {
  const [mealData, setMealData] = useState({
    name: '',
    carbs: '',
    protein: '',
    calories: '',
    fat: '',
    dateTime: new Date().toISOString().slice(0, 16)
  });

  const handleChange = (e) => {
    const { name, value } = e.target;
    setMealData(prev => ({
      ...prev,
      [name]: value
    }));
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    
    // Convert string values to integers for numeric fields
    const formattedData = {
      name: mealData.name,
      carbs: parseInt(mealData.carbs) || 0,
      protein: parseInt(mealData.protein) || 0,
      calories: parseInt(mealData.calories) || 0,
      fat: parseInt(mealData.fat) || 0,
      dateTime: mealData.dateTime
    };
    
    console.log('Meal submitted:', formattedData);
    // Add your API call or data handling here
    
    // Reset form
    setMealData({
      name: '',
      carbs: '',
      protein: '',
      calories: '',
      fat: '',
      dateTime: new Date().toISOString().slice(0, 16)
    });
  };

  return (
    <div className="add-meal-container">
      <h2>Add Meal</h2>
      <form onSubmit={handleSubmit} className="meal-form">
        <div className="form-group">
          <label htmlFor="name">Meal Name</label>
          <input
            type="text"
            id="name"
            name="name"
            value={mealData.name}
            onChange={handleChange}
            required
            placeholder="Enter meal name"
          />
        </div>

        <div className="form-group">
          <label htmlFor="carbs">Carbs (g)</label>
          <input
            type="number"
            id="carbs"
            name="carbs"
            value={mealData.carbs}
            onChange={handleChange}
            required
            min="0"
            placeholder="0"
          />
        </div>

        <div className="form-group">
          <label htmlFor="protein">Protein (g)</label>
          <input
            type="number"
            id="protein"
            name="protein"
            value={mealData.protein}
            onChange={handleChange}
            required
            min="0"
            placeholder="0"
          />
        </div>

        <div className="form-group">
          <label htmlFor="calories">Calories</label>
          <input
            type="number"
            id="calories"
            name="calories"
            value={mealData.calories}
            onChange={handleChange}
            required
            min="0"
            placeholder="0"
          />
        </div>

        <div className="form-group">
          <label htmlFor="fat">Fat (g)</label>
          <input
            type="number"
            id="fat"
            name="fat"
            value={mealData.fat}
            onChange={handleChange}
            required
            min="0"
            placeholder="0"
          />
        </div>

        <div className="form-group">
          <label htmlFor="dateTime">Date & Time</label>
          <input
            type="datetime-local"
            id="dateTime"
            name="dateTime"
            value={mealData.dateTime}
            onChange={handleChange}
            required
          />
        </div>

        <button type="submit" className="submit-btn">Add Meal</button>
      </form>
    </div>
  );
}

export default AddMeal;