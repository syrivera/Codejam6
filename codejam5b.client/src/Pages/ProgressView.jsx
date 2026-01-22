import { useState, useEffect } from 'react';
import './ProgressView.css';

function ProgressView() {
  const [progressData, setProgressData] = useState(null);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState(null);
  const [showModal, setShowModal] = useState(false);
  const [formData, setFormData] = useState({
    currentWeight: 0,
    targetWeight: 0,
    targetDailyCalories: 0,
    targetDailyCarbs: 0,
    targetDailyFat: 0,
    targetDailyProtein: 0
  });

  useEffect(() => {
    fetchProgressData();

    const intervalId = setInterval(fetchProgressData, 10000);
    
    const handleMealAdded = () => fetchProgressData();
    window.addEventListener('mealAdded', handleMealAdded);

    return () => {
      clearInterval(intervalId);
      window.removeEventListener('mealAdded', handleMealAdded);
    };
  }, []);

  const fetchProgressData = async () => {
    if (!progressData) {
      setIsLoading(true);
    }
    setError(null);

    try {
      const response = await fetch('/api/progress');
      if (!response.ok) {
        throw new Error('Failed to fetch progress data');
      }
      const data = await response.json();
      setProgressData(data);
      
    } catch (err) {
      setError(err.message);
    } finally {
      setIsLoading(false);
    }
  };
//modify stats 
  const handleModifyGoals = () => {
    setFormData({
      currentWeight: progressData.currentWeight,
      targetWeight: progressData.targetWeight,
      targetDailyCalories: progressData.targetDailyCalories,
      targetDailyCarbs: progressData.targetDailyCarbs,
      targetDailyFat: progressData.targetDailyFat,
      targetDailyProtein: progressData.targetDailyProtein
    });
    setShowModal(true);
  };

  const handleCloseModal = () => {
    setShowModal(false);
  };

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({ ...prev, [name]: parseInt(value) || 0 }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    
    try {
      const response = await fetch('/api/progress', {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(formData)
      });
      
      if (!response.ok) {
        throw new Error('Failed to update goals');
      }
      
      await fetchProgressData();
      setShowModal(false);
      alert('Goals updated successfully!');
    } catch (err) {
      alert('Error updating goals: ' + err.message);
    }
  };

  if (isLoading) {
    return (
      <div className="progress-view-container">
        <h2>Progress Stats</h2>
        <div className="loading">Loading...</div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="progress-view-container">
        <h2>Progress Stats</h2>
        <div className="error-message">
          <p>Error: {error}</p>
          <button onClick={fetchProgressData} className="retry-btn">
            Retry
          </button>
        </div>
      </div>
    );
  }

  if (!progressData) {
    return (
      <div className="progress-view-container">
        <h2>Progress Stats</h2>
        <p>No progress data available.</p>
      </div>
    );
  }

  const weightDifference = progressData.currentWeight - progressData.targetWeight;
  const weightProgress = weightDifference > 0 ? 'to lose' : 'to gain';

  // Calculate percentages for progress bars
  const calcPercentage = (consumed, target) => Math.min((consumed / target) * 100, 100);
  const calcRemaining = (consumed, target) => Math.max(target - consumed, 0);

  const caloriesPercent = calcPercentage(progressData.consumedCalories, progressData.targetDailyCalories);
  const carbsPercent = calcPercentage(progressData.consumedCarbs, progressData.targetDailyCarbs);
  const fatPercent = calcPercentage(progressData.consumedFat, progressData.targetDailyFat);
  const proteinPercent = calcPercentage(progressData.consumedProtein, progressData.targetDailyProtein);

  return (
    <div className="progress-view-container">
      <h2>Progress Stats</h2>
      
      <div className="stats-grid">
        <div className="stat-card weight-card">
          <div className="stat-header">
            <h3>Weight</h3>
          </div>
          <div className="weight-stats">
            <div className="weight-item">
              <span className="label">Current</span>
              <span className="value">{progressData.currentWeight} lbs</span>
            </div>
            <div className="weight-item">
              <span className="label">Target</span>
              <span className="value">{progressData.targetWeight} lbs</span>
            </div>
            <div className="weight-progress">
              <span className="progress-text">
                {Math.abs(weightDifference)} lbs {weightProgress}
              </span>
            </div>
          </div>
        </div>

        <div className="stat-card nutrition-card">
          <div className="stat-header">
            <h3>Today's Nutrition Goals</h3>
          </div>
          <div className="progress-list">
            <div className="progress-item">
              <div className="progress-header">
                <span className="progress-label">Calories</span>
                <span className="progress-values">{progressData.consumedCalories} / {progressData.targetDailyCalories} kcal</span>
              </div>
              <div className="progress-bar-wrapper">
                <div className="progress-bar-fill calories-bar" style={{width: `${caloriesPercent}%`}}></div>
              </div>
            </div>

            <div className="progress-item">
              <div className="progress-header">
                <span className="progress-label">Protein</span>
                <span className="progress-values">{progressData.consumedProtein} / {progressData.targetDailyProtein} g</span>
              </div>
              <div className="progress-bar-wrapper">
                <div className="progress-bar-fill protein-bar" style={{width: `${proteinPercent}%`}}></div>
              </div>
            </div>

            <div className="progress-item">
              <div className="progress-header">
                <span className="progress-label">Carbs</span>
                <span className="progress-values">{progressData.consumedCarbs} / {progressData.targetDailyCarbs} g</span>
              </div>
              <div className="progress-bar-wrapper">
                <div className="progress-bar-fill carbs-bar" style={{width: `${carbsPercent}%`}}></div>
              </div>
            </div>

            <div className="progress-item">
              <div className="progress-header">
                <span className="progress-label">Fat</span>
                <span className="progress-values">{progressData.consumedFat} / {progressData.targetDailyFat} g</span>
              </div>
              <div className="progress-bar-wrapper">
                <div className="progress-bar-fill fat-bar" style={{width: `${fatPercent}%`}}></div>
              </div>
            </div>
          </div>
        </div>
      </div>

      <div className="modify-goals-section">
        <button onClick={handleModifyGoals} className="modify-goals-btn">
          Modify Goals
        </button>
      </div>

      {showModal && (
        <div className="modal-overlay" onClick={handleCloseModal}>
          <div className="modal-content" onClick={(e) => e.stopPropagation()}>
            <h3>Modify Your Goals</h3>
            <form onSubmit={handleSubmit}>
              <div className="form-section">
                <h4>Weight Goals</h4>
                <div className="form-row">
                  <label>
                    Current Weight (lbs)
                    <input type="number" name="currentWeight" value={formData.currentWeight} onChange={handleInputChange} required />
                  </label>
                  <label>
                    Target Weight (lbs)
                    <input type="number" name="targetWeight" value={formData.targetWeight} onChange={handleInputChange} required />
                  </label>
                </div>
              </div>

              <div className="form-section">
                <h4>Daily Nutrition Targets</h4>
                <div className="form-row">
                  <label>
                    Calories (kcal)
                    <input type="number" name="targetDailyCalories" value={formData.targetDailyCalories} onChange={handleInputChange} required />
                  </label>
                  <label>
                    Protein (g)
                    <input type="number" name="targetDailyProtein" value={formData.targetDailyProtein} onChange={handleInputChange} required />
                  </label>
                </div>
                <div className="form-row">
                  <label>
                    Carbs (g)
                    <input type="number" name="targetDailyCarbs" value={formData.targetDailyCarbs} onChange={handleInputChange} required />
                  </label>
                  <label>
                    Fat (g)
                    <input type="number" name="targetDailyFat" value={formData.targetDailyFat} onChange={handleInputChange} required />
                  </label>
                </div>
              </div>

              <div className="modal-actions">
                <button type="button" onClick={handleCloseModal} className="cancel-btn">Cancel</button>
                <button type="submit" className="save-btn">Save Changes</button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
}

export default ProgressView;
