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

  const handleSubmit = async (e) => {
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
    
    try {
      // Well structured API call to add meal
      // Routes are easy t ounderstand and maintain
      const response = await fetch('/api/meals', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(formattedData)
      });

      if (!response.ok) {
        throw new Error('Failed to add meal');
      }

      const result = await response.json();
      console.log('Meal added successfully:', result);

      // Update user progress with the meal nutrients
      try {
        // Http methods could be a bit more descriptive - For example, /get-progress or /add-progress
        const progressResponse = await fetch('/api/progress', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
          },
          body: JSON.stringify({
            calories: formattedData.calories,
            carbs: formattedData.carbs,
            fat: formattedData.fat,
            protein: formattedData.protein
          })
        });

        if (progressResponse.ok) {
          const progressData = await progressResponse.json();
          console.log('Progress updated:', progressData);
        }
      } catch (progressError) {
        console.error('Error updating progress:', progressError);
        // Don't fail the whole operation if progress update fails
      }
      
      // Reset form
      setMealData({
        name: '',
        carbs: '',
        protein: '',
        calories: '',
        fat: '',
        dateTime: new Date().toISOString().slice(0, 16)
      });

      alert('Meal added successfully!');
    } catch (error) {
      console.error('Error adding meal:', error);
      alert('Failed to add meal. Please try again.');
    }
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