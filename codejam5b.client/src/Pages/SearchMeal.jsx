import { useState } from 'react';
import './SearchMeal.css';

function SearchMeal() {
  const [searchQuery, setSearchQuery] = useState('');
  const [searchResults, setSearchResults] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState(null);
  const [addingMealId, setAddingMealId] = useState(null);

  const handleAddMeal = async (meal) => {
    setAddingMealId(meal.mealId);
    
    try {
      // Update progress with the meal nutrients
      const progressResponse = await fetch('/api/progress/add-meal', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          calories: meal.calories,
          carbs: meal.carbs,
          fat: meal.fat,
          protein: meal.protein
        })
      });

      if (!progressResponse.ok) {
        throw new Error('Failed to update progress');
      }

      const progressData = await progressResponse.json();
      console.log('Progress updated:', progressData);
      alert(`${meal.name} added to your daily intake!`);
      
    } catch (error) {
      console.error('Error adding meal:', error);
      alert('Failed to add meal to progress. Please try again.');
    } finally {
      setAddingMealId(null);
    }
  };

  const handleSearch = async (e) => {
    e.preventDefault();
    
    if (!searchQuery.trim()) {
      return;
    }

    setIsLoading(true);
    setError(null);

    try {
      const response = await fetch(`/api/meals/search?name=${encodeURIComponent(searchQuery)}`);
      if (!response.ok) {
        throw new Error('Failed to fetch meals');
      }
      const data = await response.json();
      setSearchResults(data);
      
    } catch (err) {
      setError(err.message);
      setSearchResults([]);
    } finally {
      setIsLoading(false);
    }
  };

  const handleInputChange = (e) => {
    setSearchQuery(e.target.value);
  };

  const handleClear = () => {
    setSearchQuery('');
    setSearchResults([]);
    setError(null);
  };

  return (
    <div className="search-meal-container">
      <h2>Search Meals</h2>
      
      <form onSubmit={handleSearch} className="search-form">
        <div className="search-input-group">
          <input
            type="text"
            value={searchQuery}
            onChange={handleInputChange}
            placeholder="Enter meal name..."
            className="search-input"
          />
          {searchQuery && (
            <button 
              type="button" 
              onClick={handleClear}
              className="clear-btn"
              aria-label="Clear search"
            >
              ×
            </button>
          )}
        </div>
        <button 
          type="submit" 
          className="search-btn"
          disabled={isLoading || !searchQuery.trim()}
        >
          {isLoading ? 'Searching...' : 'Search'}
        </button>
      </form>

      {error && (
        <div className="error-message">
          <p>Error: {error}</p>
        </div>
      )}

      {searchResults.length > 0 && (
        <div className="results-container">
          <h3>Results ({searchResults.length})</h3>
          <div className="results-list">
            {searchResults.map((meal, index) => (
              <div key={index} className="meal-card">
                <h4>{meal.name}</h4>
                <div className="meal-details">
                  <span><strong>Calories:</strong> {meal.calories}</span>
                  <span><strong>Protein:</strong> {meal.protein}g</span>
                  <span><strong>Carbs:</strong> {meal.carbs}g</span>
                  <span><strong>Fat:</strong> {meal.fat}g</span>
                </div>
                <button 
                  className="add-meal-btn"
                  onClick={() => handleAddMeal(meal)}
                  disabled={addingMealId === meal.mealId}
                >
                  {addingMealId === meal.mealId ? 'Adding...' : 'Add to Today'}
                </button>
              </div>
            ))}
          </div>
        </div>
      )}

      {!isLoading && searchResults.length === 0 && searchQuery && !error && (
        <div className="no-results">
          <p>No meals found matching "{searchQuery}"</p>
        </div>
      )}
    </div>
  );
}

export default SearchMeal;
